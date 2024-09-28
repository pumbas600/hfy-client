import { HeadMeta } from "@/components/atomic";
import ChapterCardList from "@/components/subreddit/chapterCardList";
import SubredditLayout from "@/components/subreddit/subredditLayout";
import config from "@/config";
import { GetNewChaptersRequest, GetSubredditRequest } from "@/types/api";
import { PaginatedChapters } from "@/types/chapter";
import { Subreddit } from "@/types/subreddit";
import { Api } from "@/util/api";
import { GetServerSideProps, InferGetServerSidePropsType } from "next";

interface SubredditPageProps {
  subreddit: Subreddit;
  paginatedChapters: PaginatedChapters;
  newChaptersUrl: string;
}

const ONE_MINUTE = 60;

export const getServerSideProps = (async ({ req, res, params, query }) => {
  if (!params) {
    return { notFound: true };
  }

  const subredditUrl = `${config.api.baseUrl}/subreddits/${params.subreddit}`;
  const newChaptersUrl = new URL(
    `${config.api.baseUrl}/chapters/r/${params.subreddit}/new`
  );

  if (typeof query.q === "string" && query.q.trim() !== "") {
    newChaptersUrl.searchParams.set("title", query.q);
  }

  console.log(req.headers.cookie);

  try {
    await Api.assertAccessTokenPresent(req, res);
    const [subredditResponse, paginatedChaptersResponse] = await Promise.all([
      Api.get<GetSubredditRequest.ResBody>(subredditUrl, {
        req,
        res,
      }),
      Api.get<GetNewChaptersRequest.ResBody>(newChaptersUrl, {
        revalidate: ONE_MINUTE,
        default: { pageSize: 20, nextKey: null, data: [] },
        req,
        res,
      }),
    ]);

    return {
      props: {
        subreddit: subredditResponse.data,
        paginatedChapters: paginatedChaptersResponse.data,
        newChaptersUrl: newChaptersUrl.toString(),
      },
    };
  } catch (err) {
    console.error(err);
    return { notFound: true };
  }
}) satisfies GetServerSideProps<SubredditPageProps, { subreddit: string }>;

export default function SubredditPage({
  subreddit,
  paginatedChapters,
  newChaptersUrl,
}: InferGetServerSidePropsType<typeof getServerSideProps>) {
  return (
    <>
      <HeadMeta title={subreddit.title} description={subreddit.description} />
      <SubredditLayout subreddit={subreddit}>
        <ChapterCardList
          paginatedChapters={paginatedChapters}
          endpointUrl={newChaptersUrl}
        />
      </SubredditLayout>
    </>
  );
}
