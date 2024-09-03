import ChapterButtons from "@/components/buttons/chapterButtons";
import ChapterText from "@/components/chapterText";
import Container from "@/components/container";
import ChapterHeader from "@/components/layout/chapterHeader";
import PageFooter from "@/components/layout/pageFooter";
import config from "@/config";
import { GetChapterRequest } from "@/types/api";
import { Params } from "@/types/next";

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
    <article>
      <ChapterHeader chapter={chapter} />
      <Container>
        <ChapterButtons chapter={chapter} />
        <ChapterText textHtml={chapter.textHtml} />
        <ChapterButtons chapter={chapter} hideFirstLink />
      </Container>
      <PageFooter />
    </article>
  );
}
