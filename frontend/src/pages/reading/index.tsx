import { HeadMeta } from "@/components/atomic";
import {
  Aside,
  Header,
  Main,
  PageLayout,
} from "@/components/layout/pageLayout";
import ChapterSummaryCard from "@/components/subreddit/chapterSummaryCard";
import config from "@/config";
import { GetReadingHistoryRequest } from "@/types/api";
import { ReadingHistoryEntry } from "@/types/history";
import { Api } from "@/util/api";
import { GetServerSideProps } from "next";

interface ReadingHistoryPageProps {
  readingHistory: ReadingHistoryEntry[];
}

export const getServerSideProps = (async ({ req, res }) => {
  const readingHistoryUrl = `${config.api.baseUrl}/history/reading`;

  try {
    await Api.assertAccessTokenPresent(req, res);
    const response = await Api.get<GetReadingHistoryRequest.ResBody>(
      readingHistoryUrl,
      { req }
    );

    return {
      props: { readingHistory: response.data },
    };
  } catch (err) {
    console.error(err);
    return {
      props: { readingHistory: [] },
    };
  }
}) satisfies GetServerSideProps<ReadingHistoryPageProps>;

export default function ReadingHistoryPage({
  readingHistory,
}: ReadingHistoryPageProps) {
  console.log(readingHistory);

  return (
    <>
      <HeadMeta title={`Reading | ${config.title}`} />
      <PageLayout>
        <Header>
          <h2>Reading</h2>
          <p>Recently read stories</p>
        </Header>
        <Aside />
        <Main>
          {readingHistory.map((entry) => (
            <ChapterSummaryCard
              key={entry.chapterMetadata.id}
              metadata={entry.chapterMetadata}
            />
          ))}
        </Main>
      </PageLayout>
    </>
  );
}
