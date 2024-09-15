import ChapterCardList from "@/components/cards/chapterCardList";
import config from "@/config";
import { GetNewChaptersRequest } from "@/types/api";
import { GetSubredditRequest } from "@/types/api/subreddit";
import { Params } from "@/types/next";
import { Api } from "@/util/api";
import { Metadata, ResolvingMetadata } from "next";
import SubredditLayout from "@/components/subreddit/subredditLayout";

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

export default async function Subreddit({
  params,
  searchParams,
}: Params<{ subreddit: string }, { q?: string }>) {
  const subredditUrl = `${config.api.baseUrl}/subreddits/${params.subreddit}`;
  const newChaptersUrl = new URL(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/new`
  );

  if (searchParams.q !== undefined && searchParams.q.trim() !== "") {
    newChaptersUrl.searchParams.set("title", searchParams.q);
  }

  const [subreddit, paginatedChapters] = await Promise.all([
    Api.get<GetSubredditRequest.ResBody>(subredditUrl),
    Api.get<GetNewChaptersRequest.ResBody>(newChaptersUrl, {
      revalidate: ONE_MINUTE,
      default: { pageSize: 20, nextKey: null, data: [] },
    }),
  ]);

  return (
    <SubredditLayout subreddit={subreddit}>
      <ChapterCardList
        paginatedChapters={paginatedChapters}
        endpointUrl={newChaptersUrl.toString()}
      />
    </SubredditLayout>
  );
}
