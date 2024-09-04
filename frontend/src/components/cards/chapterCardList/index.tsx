import { ChapterMetadata } from "@/types/chapter";
import ChapterSummaryCard from "../chapterSummaryCard";
import { Fragment } from "react";

export interface ChapterCardListProps {
  chapters: ChapterMetadata[];
}

export default function ChapterCardList({ chapters }: ChapterCardListProps) {
  return (
    <>
      {chapters.map((chapter) => (
        <Fragment key={chapter.id}>
          <ChapterSummaryCard key={chapter.id} metadata={chapter} />
          <hr />
        </Fragment>
      ))}
    </>
  );
}
