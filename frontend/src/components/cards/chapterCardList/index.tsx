"use client";

import { PaginatedChapters } from "@/types/chapter";
import ChapterSummaryCard from "../chapterSummaryCard";
import { Fragment, useState } from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import { GetNewChaptersRequest } from "@/types/api";
import config from "@/config";

export interface ChapterCardListProps {
  subreddit: string;
  paginatedChapters: PaginatedChapters;
}

function hasMoreChapters(paginatedChapters: PaginatedChapters): boolean {
  return (
    paginatedChapters.nextKey !== null &&
    paginatedChapters.data.length == paginatedChapters.pageSize
  );
}

export default function ChapterCardList({
  subreddit,
  paginatedChapters,
}: ChapterCardListProps) {
  const [nextKey, setNextKey] = useState(paginatedChapters.nextKey);
  const [chapters, setChapters] = useState(paginatedChapters.data);
  const [hasNext, setHasNext] = useState(hasMoreChapters(paginatedChapters));

  const fetchNext = async (): Promise<void> => {
    if (nextKey === null) {
      return;
    }

    console.debug("[Paginated Chapters]: Fetching more chapters");

    const requestUrl = new URL(
      `${config.api.baseUrl}/chapters/r/${subreddit}/new`
    );
    requestUrl.searchParams.set("lastCreated", nextKey.lastCreatedAtUtc);
    requestUrl.searchParams.set("lastId", nextKey.lastPostId);
    const res = await fetch(requestUrl.toString());

    const paginatedChapters: GetNewChaptersRequest.ResBody = await res.json();

    setNextKey(paginatedChapters.nextKey);
    setChapters([...chapters, ...paginatedChapters.data]);
    setHasNext(hasMoreChapters(paginatedChapters));
  };

  return (
    <InfiniteScroll
      dataLength={chapters.length}
      hasMore={hasNext}
      loader={<h4>Loading...</h4>}
      next={fetchNext}
      endMessage={
        <p style={{ textAlign: "center" }}>
          <b>Yay! You have seen it all</b>
        </p>
      }
    >
      {chapters.map((chapter) => (
        <Fragment key={chapter.id}>
          <ChapterSummaryCard key={chapter.id} metadata={chapter} />
          <hr />
        </Fragment>
      ))}
    </InfiniteScroll>
  );
}