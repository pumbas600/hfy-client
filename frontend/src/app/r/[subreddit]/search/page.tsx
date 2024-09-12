import ChapterCardList from "@/components/cards/chapterCardList";
import Container from "@/components/atomic/container";
import ChapterSearchInput from "@/components/inputs/chapterSearchInput";
import PageFooter from "@/components/layout/pageFooter";
import PageHeader from "@/components/layout/pageHeader";
import config from "@/config";
import { GetChaptersByTitleRequest } from "@/types/api";
import { Params } from "@/types/next";
import { Api } from "@/util/api";

const FIVE_MINUTES = 5 * 60;

export const revalidate = FIVE_MINUTES; // Incrementally regenerate every 5 minutes

export default async function SubredditSearch({
  params,
  searchParams,
}: Params<{ subreddit: string }, { q: string }>) {
  const endpointUrl = new URL(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/search`
  );

  endpointUrl.searchParams.set("title", searchParams.q);

  const paginatedChapters = await Api.get<GetChaptersByTitleRequest.ResBody>(
    endpointUrl,
    {
      revalidate: FIVE_MINUTES,
      default: { pageSize: 20, nextKey: null, data: [] },
    }
  );

  return (
    <div>
      <PageHeader backLink={`/r/${params.subreddit}`}>
        <h1>r/{params.subreddit}</h1>
        <ChapterSearchInput
          subreddit={params.subreddit}
          defaultValue={searchParams.q}
        />
      </PageHeader>
      <Container main>
        <ChapterCardList
          paginatedChapters={paginatedChapters}
          endpointUrl={endpointUrl.toString()}
        />
      </Container>
      <PageFooter />
    </div>
  );
}
