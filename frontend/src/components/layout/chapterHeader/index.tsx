import { FullChapter } from "@/types/chapter";
import PageHeader from "../pageHeader";
import styles from "./chapterHeader.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";

export interface ChapterHeaderProps {
  chapter: FullChapter;
}

export default function ChapterHeader({ chapter }: ChapterHeaderProps) {
  return (
    <PageHeader>
      <div className={styles.detailsContainer}>
        <div className={styles.authorContainer}>
          <strong>r/{chapter.subreddit}</strong>
          <a href={chapter.redditAuthorLink}>{chapter.author}</a>
        </div>
        <a href={chapter.redditPostLink} title="Read on Reddit">
          <FontAwesomeIcon
            icon={faReddit}
            size="lg"
            className={styles.redditIcon}
          />
        </a>
      </div>
      <h2>{chapter.title}</h2>
      <time>{/*Created at ...*/}</time>
    </PageHeader>
  );
}
