import ChapterButtons from "@/components/buttons/chapterButtons";
import Container from "@/components/container";
import ChapterHeader from "@/components/layout/chapterHeader";
import PageFooter from "@/components/layout/pageFooter";
import TextLayout from "@/components/layout/textLayout";
import config from "@/config";
import { GetChapterRequest } from "@/types/api";
import { Params } from "@/types/next";

export const revalidate = 5 * 60; // Incrementally regenerate every 5 minute

export default async function Page({ params }: Params<{ id: string }>) {
  const res = await fetch(`${config.api.baseUrl}/chapters/${params.id}`);
  const chapter: GetChapterRequest.ResBody = await res.json();

  if (!res.ok) {
    return (
      <div>
        <h2>Error!</h2>
        <p>{JSON.stringify(chapter, undefined, 4)}</p>
      </div>
    );
  }

  return (
    <div>
      <ChapterHeader chapter={chapter} />
      <Container>
        <ChapterButtons chapter={chapter} />
        <TextLayout textHtml={chapter.textHtml} />
        <ChapterButtons chapter={chapter} hideFirstLink />
      </Container>
      <PageFooter />
    </div>
  );
}
