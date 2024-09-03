import ChapterSummaryCard from "@/components/cards/chapterSummaryCard";
import Container from "@/components/container";
import config from "@/config";
import { GetNewChaptersRequest } from "@/types/api";
import { Params } from "@/types/next";

export default async function Page({ params }: Params<{ subreddit: string }>) {
  const res = await fetch(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/new`
  );
  const paginatedChapters: GetNewChaptersRequest.ResBody = await res.json();

  console.log(paginatedChapters);

  const hasNext = paginatedChapters.data.length != paginatedChapters.pageSize;

  return (
    <Container>
      <main>
        <h1>{params.subreddit}</h1>
        {paginatedChapters.data.map((chapter) => (
          <ChapterSummaryCard key={chapter.id} metadata={chapter} />
        ))}
      </main>
    </Container>
  );
}
