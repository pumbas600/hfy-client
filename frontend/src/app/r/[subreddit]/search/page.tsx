import ChapterCardList from "@/components/cards/chapterCardList";
import Container from "@/components/container";
import SearchInput from "@/components/inputs/searchInput";
import PageFooter from "@/components/layout/pageFooter";
import PageHeader from "@/components/layout/pageHeader";
import config from "@/config";
import { GetChaptersByTitleRequest } from "@/types/api";
import { Params } from "@/types/next";

const FIVE_MINUTES = 5 * 60;

export const reinvalidate = FIVE_MINUTES; // Incrementally regenerate every 5 minutes

export default async function SubredditSearch({
  params,
}: Params<{ subreddit: string; q: string }>) {
  const endpointUrl = new URL(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/search`
  );

  endpointUrl.searchParams.set("title", params.q);

  const res = await fetch(endpointUrl.toString(), {
    next: { revalidate: FIVE_MINUTES },
  });

  const paginatedChapters: GetChaptersByTitleRequest.ResBody = await res.json();

  return (
    <div>
      <PageHeader>
        <h1>r/{params.subreddit}</h1>
        <SearchInput
          name="q"
          placeholder="Search chapters..."
          aria-label="Search for chapters by their title"
        />
      </PageHeader>
      <Container main>
        <ChapterCardList
          paginatedChapters={paginatedChapters}
          endpointUrl={endpointUrl}
        />
      </Container>
      <PageFooter />
    </div>
  );
}
