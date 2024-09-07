import ChapterCardList from "@/components/cards/chapterCardList";
import Container from "@/components/container";
import ChapterSearchInput from "@/components/inputs/chapterSearchInput";
import PageFooter from "@/components/layout/pageFooter";
import PageHeader from "@/components/layout/pageHeader";
import config from "@/config";
import { GetNewChaptersRequest } from "@/types/api";
import { Params } from "@/types/next";

const ONE_MINUTE = 60;

export const revalidate = ONE_MINUTE; // Incrementally regenerate every 1 minute

export default async function Subreddit({
  params,
}: Params<{ subreddit: string }>) {
  const endpointUrl = new URL(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/new`
  );

  const res = await fetch(endpointUrl.toString(), {
    next: { revalidate: ONE_MINUTE },
  });
  const paginatedChapters: GetNewChaptersRequest.ResBody = await res.json();

  return (
    <div>
      <PageHeader>
        <h1>r/{params.subreddit}</h1>
        <ChapterSearchInput subreddit={params.subreddit} />
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
