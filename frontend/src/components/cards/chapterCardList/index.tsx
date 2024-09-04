import { ChapterMetadata } from "@/types/chapter";
import ChapterSummaryCard from "../chapterSummaryCard";

export interface ChapterCardListProps {
  chapters: ChapterMetadata[];
}

export default function ChapterCardList({ chapters }: ChapterCardListProps) {
  return (
    <>
      {chapters.map((chapter) => (
        <>
          <ChapterSummaryCard key={chapter.id} metadata={chapter} />
          <hr />
        </>
      ))}
    </>
  );
}
