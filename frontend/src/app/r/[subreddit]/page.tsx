import ChapterCardList from "@/components/cards/chapterCardList";
import ChapterSummaryCard from "@/components/cards/chapterSummaryCard";
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

  const hasNext = paginatedChapters.data.length != paginatedChapters.pageSize;

  return (
    <div>
      <PageHeader>
        <h1>r/{params.subreddit}</h1>
      </PageHeader>
      <Container>
        <ChapterCardList chapters={paginatedChapters.data} />
      </Container>
      <PageFooter />
    </div>
  );
}
