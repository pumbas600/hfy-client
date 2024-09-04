import { ChapterMetadata } from "@/types/chapter";
import ChapterSummaryCard from "../chapterSummaryCard";
import styles from "./chapterCardList.module.css";

export interface ChapterCardListProps {
  chapters: ChapterMetadata[];
}

export default function ChapterCardList({ chapters }: ChapterCardListProps) {
  return (
    <main className={styles.chapterCardList}>
      {chapters.map((chapter) => (
        <>
          <ChapterSummaryCard key={chapter.id} metadata={chapter} />
          <hr />
        </>
      ))}
    </main>
  );
}
