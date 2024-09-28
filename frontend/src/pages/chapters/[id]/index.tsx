import { HeadMeta } from "@/components/atomic";
import ChapterButtons from "@/components/chapter/chapterButtons";
import ChapterLayout from "@/components/chapter/chapterLayout";
import ScrollToTopButton from "@/components/composite/scrollToTopButton";
import TextLayout from "@/components/layout/textLayout";
import config from "@/config";
import { GetChapterRequest } from "@/types/api";
import { FullChapter } from "@/types/chapter";
import { Api } from "@/util/api";
import { GetServerSideProps } from "next";

export interface ChapterPageProps {
  chapter: FullChapter;
}

const FIVE_MINUTES = 5 * 60;

export const getServerSideProps = (async ({ req, res, params }) => {
  if (!params) {
    return { notFound: true };
  }

  const chapterResponse = await Api.get<GetChapterRequest.ResBody>(
    `${config.api.baseUrl}/chapters/${params.id}`,
    {
      revalidate: FIVE_MINUTES,
      req,
      res,
    }
  );

  return { props: { chapter: chapterResponse.data } };
}) satisfies GetServerSideProps<ChapterPageProps, { id: string }>;

export default function ChapterPage({ chapter }: ChapterPageProps) {
  return (
    <>
      <HeadMeta
        title={chapter.title}
        image={chapter.coverArtUrl}
        imageAlt={`${chapter.title} cover art`}
      />
      <ChapterLayout chapter={chapter}>
        <ChapterButtons chapter={chapter} />
        <TextLayout textHtml={chapter.textHtml} />
        <ChapterButtons chapter={chapter} hideFirstLink />
        <ScrollToTopButton />
      </ChapterLayout>
    </>
  );
}
