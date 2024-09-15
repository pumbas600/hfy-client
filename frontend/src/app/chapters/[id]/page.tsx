import ChapterButtons from "@/components/buttons/chapterButtons";
import ScrollToTopButton from "@/components/buttons/scrollToTopButton";
import Container from "@/components/atomic/container";
import ChapterHeader from "@/components/layout/chapterHeader";
import PageFooter from "@/components/layout/pageFooter";
import TextLayout from "@/components/layout/textLayout";
import config from "@/config";
import { GetChapterRequest } from "@/types/api";
import { Params } from "@/types/next";
import { Api } from "@/util/api";
import { Metadata, ResolvingMetadata } from "next";
import ChapterLayout from "@/components/chapter/chapterLayout";

const FIVE_MINUTES = 5 * 60;

export const revalidate = FIVE_MINUTES; // Incrementally regenerate every 5 minute

export async function generateMetadata(
  { params }: Params<{ id: string }>,
  parent: ResolvingMetadata
): Promise<Metadata> {
  try {
    const chapter = await Api.get<GetChapterRequest.ResBody>(
      `${config.api.baseUrl}/chapters/${params.id}`,
      { revalidate: FIVE_MINUTES }
    );

    const images: string[] = [];
    if (chapter.coverArtUrl) {
      images.push(chapter.coverArtUrl);
    }

    return {
      title: chapter.title,
      authors: [{ name: chapter.author, url: chapter.redditAuthorLink }],
      openGraph: {
        images: images,
      },
    };
  } catch (error) {
    const parentMetadata = await parent;
    return {
      title: parentMetadata.title,
      openGraph: {
        images: parentMetadata.openGraph?.images,
      },
    };
  }
}

export default async function Page({ params }: Params<{ id: string }>) {
  let chapter: GetChapterRequest.ResBody;

  try {
    chapter = await Api.get<GetChapterRequest.ResBody>(
      `${config.api.baseUrl}/chapters/${params.id}`,
      { revalidate: FIVE_MINUTES }
    );
  } catch (error) {
    if (error instanceof Error) {
      return (
        <div>
          <h2>Error!</h2>
          <p>{error.message}</p>
        </div>
      );
    }

    return null;
  }

  return (
    <ChapterLayout chapter={chapter}>
      <ChapterButtons chapter={chapter} />
      <TextLayout textHtml={chapter.textHtml} />
      <ChapterButtons chapter={chapter} hideFirstLink />
      <ScrollToTopButton />
    </ChapterLayout>
  );
}
