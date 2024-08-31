import { FullChapter } from "@/types/chapter";
import PageHeader from "../pageHeader";
import styles from "./chapterHeader.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";
import dayjs from "dayjs";
import RelativeTime from "@/components/times/relativeTime";

export interface ChapterHeaderProps {
  chapter: FullChapter;
}

export default function ChapterHeader({ chapter }: ChapterHeaderProps) {
  const createdAt = dayjs(chapter.createdAtUtc);
  const processedAt = dayjs(chapter.processedAtUtc);

  return (
    <PageHeader>
      <div className={styles.detailsContainer}>
        <div className={styles.authorContainer}>
          <strong>r/{chapter.subreddit}</strong>
          <a href={chapter.redditAuthorLink}>{chapter.author}</a>
        </div>
        <a
          href={chapter.redditPostLink}
          title="Read on Reddit"
          className={styles.redditIcon}
        >
          <FontAwesomeIcon icon={faReddit} size="lg" />
        </a>
      </div>
      <h2>{chapter.title}</h2>
      <p className={styles.timeMetadata}>
        Posted <RelativeTime dateTimeUtc={chapter.createdAtUtc} /> • Last synced{" "}
        <RelativeTime dateTimeUtc={chapter.processedAtUtc} />
      </p>
    </PageHeader>
  );
}
