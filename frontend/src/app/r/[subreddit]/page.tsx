import ChapterCardList from "@/components/cards/chapterCardList";
import { Container } from "@/components/atomic";
import PageFooter from "@/components/layout/pageFooter";
import config from "@/config";
import { GetNewChaptersRequest } from "@/types/api";
import { GetSubredditRequest } from "@/types/api/subreddit";
import { Params } from "@/types/next";
import { Api } from "@/util/api";
import SubredditHeader from "@/components/subreddit/subredditHeader";

const ONE_MINUTE = 60;

export const revalidate = ONE_MINUTE; // Incrementally regenerate every 1 minute

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
    Api.get<GetSubredditRequest.ResBody>(subredditUrl, {
      revalidate: ONE_MINUTE,
    }),
    Api.get<GetNewChaptersRequest.ResBody>(newChaptersUrl, {
      revalidate: ONE_MINUTE,
      default: { pageSize: 20, nextKey: null, data: [] },
    }),
  ]);

  return (
    <div>
      <SubredditHeader subreddit={subreddit} />
      <Container main>
        <ChapterCardList
          paginatedChapters={paginatedChapters}
          endpointUrl={newChaptersUrl.toString()}
        />
      </Container>
      <PageFooter />
    </div>
  );
}
