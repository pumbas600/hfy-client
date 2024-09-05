import ChapterCardList from "@/components/cards/chapterCardList";
import Container from "@/components/container";
import PageFooter from "@/components/layout/pageFooter";
import PageHeader from "@/components/layout/pageHeader";
import config from "@/config";
import { GetNewChaptersRequest } from "@/types/api";
import { Params } from "@/types/next";

export const revalidate = 60; // Incrementally regenerate every 1 minute

export default async function Subreddit({
  params,
}: Params<{ subreddit: string }>) {
  const res = await fetch(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/new`
  );
  const paginatedChapters: GetNewChaptersRequest.ResBody = await res.json();

  return (
    <div>
      <PageHeader>
        <h1>r/{params.subreddit}</h1>
      </PageHeader>
      <Container main>
        <ChapterCardList
          subreddit={params.subreddit}
          paginatedChapters={paginatedChapters}
        />
      </Container>
      <PageFooter />
    </div>
  );
}