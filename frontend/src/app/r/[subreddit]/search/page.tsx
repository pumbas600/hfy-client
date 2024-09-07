import ChapterCardList from "@/components/cards/chapterCardList";
import Container from "@/components/container";
import ChapterSearchInput from "@/components/inputs/chapterSearchInput";
import PageFooter from "@/components/layout/pageFooter";
import PageHeader from "@/components/layout/pageHeader";
import config from "@/config";
import { GetChaptersByTitleRequest } from "@/types/api";
import { Params } from "@/types/next";

const FIVE_MINUTES = 5 * 60;

export const reinvalidate = FIVE_MINUTES; // Incrementally regenerate every 5 minutes

export default async function SubredditSearch({
  params,
  searchParams,
}: Params<{ subreddit: string }, { q: string }>) {
  console.log(searchParams.q);

  const endpointUrl = new URL(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/search`
  );

  endpointUrl.searchParams.set("title", searchParams.q);

  const res = await fetch(endpointUrl.toString(), {
    next: { revalidate: FIVE_MINUTES },
  });

  const paginatedChapters: GetChaptersByTitleRequest.ResBody = await res.json();

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