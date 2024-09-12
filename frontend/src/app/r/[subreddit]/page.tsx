import ChapterCardList from "@/components/cards/chapterCardList";
import Container from "@/components/container";
import ChapterSearchInput from "@/components/inputs/chapterSearchInput";
import PageFooter from "@/components/layout/pageFooter";
import PageHeader from "@/components/layout/pageHeader";
import config from "@/config";
import { GetNewChaptersRequest } from "@/types/api";
import { GetSubredditRequest } from "@/types/api/subreddit";
import { Params } from "@/types/next";
import { Api } from "@/util/api";

const ONE_MINUTE = 60;

export const revalidate = ONE_MINUTE; // Incrementally regenerate every 1 minute

export default async function Subreddit({
  params,
}: Params<{ subreddit: string }>) {
  const subredditUrl = `${config.api.baseUrl}/subreddits/${params.subreddit}`;
  const newChaptersUrl = `${config.api.baseUrl}/chapters/r/${params.subreddit}/new`;

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
      <PageHeader>
        <img src={subreddit.iconUrl} alt={`${subreddit.name}'s icon`} />
        <h2>r/{params.subreddit}</h2>
        <h3>{subreddit.title}</h3>
        <ChapterSearchInput subreddit={params.subreddit} />
      </PageHeader>
      <Container main>
        <ChapterCardList
          paginatedChapters={paginatedChapters}
          endpointUrl={newChaptersUrl}
        />
      </Container>
      <PageFooter />
    </div>
  );
}
