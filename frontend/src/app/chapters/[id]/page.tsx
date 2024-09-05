import ChapterButtons from "@/components/buttons/chapterButtons";
import ScrollToTopButton from "@/components/buttons/scrollToTopButton";
import Container from "@/components/container";
import ChapterHeader from "@/components/layout/chapterHeader";
import PageFooter from "@/components/layout/pageFooter";
import TextLayout from "@/components/layout/textLayout";
import config from "@/config";
import { GetChapterRequest } from "@/types/api";
import { Params } from "@/types/next";
import { Metadata, ResolvingMetadata } from "next";

export const revalidate = 5 * 60; // Incrementally regenerate every 5 minute

export async function generateMetadata(
  { params }: Params<{ id: string }>,
  parent: ResolvingMetadata
): Promise<Metadata> {
  const res = await fetch(`${config.api.baseUrl}/chapters/${params.id}`);
  const chapter: GetChapterRequest.ResBody = await res.json();

  const images: string[] = [];
  if (chapter.coverArtUrl) {
    images.push(chapter.coverArtUrl);
  }

  return {
    title: chapter.title,
    openGraph: {
      images: images,
    },
  };
}

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
        <ScrollToTopButton />
      </Container>
      <PageFooter />
    </div>
  );
}
