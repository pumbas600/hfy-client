import ChapterButtons from "@/components/chapter/chapterButtons";
import ChapterLayout from "@/components/chapter/chapterLayout";
import ScrollToTopButton from "@/components/composite/scrollToTopButton";
import TextLayout from "@/components/layout/textLayout";
import config from "@/config";
import { GetChapterRequest } from "@/types/api";
import { FullChapter } from "@/types/chapter";
import { Api } from "@/util/api";
import { GetServerSideProps } from "next";
import Head from "next/head";

export interface ChapterPageProps {
  chapter: FullChapter;
}

const FIVE_MINUTES = 5 * 60;

export const getServerSideProps = (async ({ params }) => {
  if (!params) {
    return { notFound: true };
  }

  const chapter = await Api.get<GetChapterRequest.ResBody>(
    `${config.api.baseUrl}/chapters/${params.id}`,
    { revalidate: FIVE_MINUTES }
  );

  return { props: { chapter } };
}) satisfies GetServerSideProps<ChapterPageProps, { id: string }>;

export default function ChapterPage({ chapter }: ChapterPageProps) {
  return (
    <>
      <Head>
        <title>{chapter.title}</title>
        {chapter.coverArtUrl && (
          <>
            <meta name="og:image" content={chapter.coverArtUrl} />
            <meta name="og:image:alt" content={`${chapter.title} cover art`} />
          </>
        )}
      </Head>
      <ChapterLayout chapter={chapter}>
        <ChapterButtons chapter={chapter} />
        <TextLayout textHtml={chapter.textHtml} />
        <ChapterButtons chapter={chapter} hideFirstLink />
        <ScrollToTopButton />
      </ChapterLayout>
    </>
  );
}
