import ChapterCardList from "@/components/subreddit/chapterCardList";
import config from "@/config";
import { GetNewChaptersRequest, GetSubredditRequest } from "@/types/api";
import { Params } from "@/types/next";
import { Api } from "@/util/api";
import { Metadata, ResolvingMetadata } from "next";
import SubredditLayout from "@/components/subreddit/subredditLayout";
import { PaginatedChapters } from "@/types/chapter";
import { Subreddit } from "@/types/subreddit";
import { cookies } from "next/headers";

const ONE_MINUTE = 60;

export const revalidate = ONE_MINUTE; // Incrementally regenerate every 1 minute

export async function generateMetadata(
  { params }: Params<{ subreddit: string }>,
  parent: ResolvingMetadata
): Promise<Metadata> {
  try {
    const subreddit = await Api.get<GetSubredditRequest.ResBody>(
      `${config.api.baseUrl}/subreddits/${params.subreddit}`
    );

    return {
      title: subreddit.title,
      description: subreddit.description,
    };
  } catch (error) {
    const parentMetadata = await parent;
    return {
      title: parentMetadata.title,
      description: parentMetadata.description,
    };
  }
}

export default async function SubredditPage({
  params,
  searchParams,
}: Params<{ subreddit: string }, { q?: string }>) {
  const cookieStore = cookies();
  console.log(cookieStore.get("RefreshToken"));
  console.log(cookieStore.get("AccessToken"));

  const subredditUrl = `${config.api.baseUrl}/subreddits/${params.subreddit}`;
  const newChaptersUrl = new URL(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/new`
  );

  if (searchParams.q !== undefined && searchParams.q.trim() !== "") {
    newChaptersUrl.searchParams.set("title", searchParams.q);
  }

  let subreddit: Subreddit;
  let paginatedChapters: PaginatedChapters;
  try {
    [subreddit, paginatedChapters] = await Promise.all([
      Api.get<GetSubredditRequest.ResBody>(subredditUrl),
      Api.get<GetNewChaptersRequest.ResBody>(newChaptersUrl, {
        revalidate: ONE_MINUTE,
        default: { pageSize: 20, nextKey: null, data: [] },
      }),
    ]);
  } catch (err) {
    return <div>Failed to load subreddit</div>;
  }

  return (
    <SubredditLayout subreddit={subreddit}>
      <ChapterCardList
        paginatedChapters={paginatedChapters}
        endpointUrl={newChaptersUrl.toString()}
      />
    </SubredditLayout>
  );
}
