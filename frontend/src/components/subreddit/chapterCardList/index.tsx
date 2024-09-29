"use client";

import { PaginatedChapters } from "@/types/chapter";
import ChapterSummaryCard from "../chapterSummaryCard";
import { Fragment, useEffect, useState } from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import { GetNewChaptersRequest } from "@/types/api";
import { Api } from "@/util/api";

export interface ChapterCardListProps {
  paginatedChapters: PaginatedChapters;
  endpointUrl: string;
}

const ONE_MINUTES = 60;

function hasMoreChapters(paginatedChapters: PaginatedChapters): boolean {
  return (
    paginatedChapters.nextKey !== null &&
    paginatedChapters.data.length == paginatedChapters.pageSize
  );
}

export default function ChapterCardList({
  paginatedChapters,
  endpointUrl,
}: ChapterCardListProps) {
  const [nextKey, setNextKey] = useState(paginatedChapters.nextKey);
  const [chapters, setChapters] = useState(paginatedChapters.data);
  const [hasNext, setHasNext] = useState(hasMoreChapters(paginatedChapters));

  useEffect(() => {
    setNextKey(paginatedChapters.nextKey);
    setChapters(paginatedChapters.data);
    setHasNext(hasMoreChapters(paginatedChapters));
  }, [paginatedChapters]);

  const fetchNext = async (): Promise<void> => {
    if (nextKey === null) {
      return;
    }

    console.debug("[Paginated Chapters]: Fetching more chapters");

    const requestUrl = new URL(endpointUrl);
    requestUrl.searchParams.set("lastCreated", nextKey.lastCreatedAtUtc);
    requestUrl.searchParams.set("lastId", nextKey.lastPostId);

    try {
      const { data: paginatedChapters } =
        await Api.get<GetNewChaptersRequest.ResBody>(requestUrl.toString(), {
          revalidate: ONE_MINUTES,
        });

      setNextKey(paginatedChapters.nextKey);
      setChapters([...chapters, ...paginatedChapters.data]);
      setHasNext(hasMoreChapters(paginatedChapters));
    } catch (err) {}
  };

  return (
    <InfiniteScroll
      dataLength={chapters.length}
      hasMore={hasNext}
      loader={<h4>Loading...</h4>}
      next={fetchNext}
      endMessage={
        <p style={{ textAlign: "center" }}>
          <strong>You have read it all!</strong>
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
