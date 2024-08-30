import { FullChapter } from "@/types/chapter";
import PageHeader from "../pageHeader";
import styles from "./chapterHeader.module.css";

export interface ChapterHeaderProps {
  chapter: FullChapter;
}

export default function ChapterHeader({ chapter }: ChapterHeaderProps) {
  return (
    <PageHeader>
      <div className={styles.authorContainer}>
        <strong>r/{chapter.subreddit}</strong>
        <p>{chapter.author}</p>
      </div>
      <h2>{chapter.title}</h2>
      <a href={chapter.redditPostLink}>Read on Reddit</a>

      <time>{/*Created at ...*/}</time>
    </PageHeader>
  );
}
