import { HeadMeta } from "@/components/atomic";
import ChapterCardList from "@/components/subreddit/chapterCardList";
import SubredditLayout from "@/components/subreddit/subredditLayout";
import config from "@/config";
import {
  GetNewChaptersRequest,
  GetSelf,
  GetSubredditRequest,
} from "@/types/api";
import { PaginatedChapters } from "@/types/chapter";
import { Subreddit } from "@/types/subreddit";
import { User } from "@/types/user";
import { Api } from "@/util/api";
import { GetServerSideProps, InferGetServerSidePropsType } from "next";

interface SubredditPageProps {
  subreddit: Subreddit;
  paginatedChapters: PaginatedChapters;
  newChaptersUrl: string;
  self: User;
}

const ONE_MINUTE = 60;
const THIRTY_MINUTES = 30 * 60;

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

  try {
    await Api.assertAccessTokenPresent(req, res);
    const [subredditResponse, paginatedChaptersResponse, selfResponse] =
      await Promise.all([
        Api.get<GetSubredditRequest.ResBody>(subredditUrl, { req }),
        Api.get<GetNewChaptersRequest.ResBody>(newChaptersUrl, {
          revalidate: ONE_MINUTE,
          default: { pageSize: 20, nextKey: null, data: [] },
          req,
        }),
        Api.get<GetSelf.ResBody>(`${config.api.baseUrl}/users/@me`, {
          revalidate: THIRTY_MINUTES,
          req,
        }),
      ]);

    return {
      props: {
        subreddit: subredditResponse.data,
        paginatedChapters: paginatedChaptersResponse.data,
        newChaptersUrl: newChaptersUrl.toString(),
        self: selfResponse.data,
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
  self,
}: InferGetServerSidePropsType<typeof getServerSideProps>) {
  return (
    <>
      <HeadMeta title={subreddit.title} description={subreddit.description} />
      <SubredditLayout subreddit={subreddit} self={self}>
        <ChapterCardList
          paginatedChapters={paginatedChapters}
          endpointUrl={newChaptersUrl}
        />
      </SubredditLayout>
    </>
  );
}
