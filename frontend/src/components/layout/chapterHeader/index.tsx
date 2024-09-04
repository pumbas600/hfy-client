import { FullChapter } from "@/types/chapter";
import PageHeader from "../pageHeader";
import styles from "./chapterHeader.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";
import Link from "next/link";

export interface ChapterHeaderProps {
  chapter: FullChapter;
}

export default function ChapterHeader({ chapter }: ChapterHeaderProps) {
  return (
    <PageHeader className={styles.chapterHeader}>
      {chapter.coverArtUrl && (
        <img src={chapter.coverArtUrl} alt={`${chapter.title}'s cover art`} />
      )}
      <div>
        <div className={styles.titleContainer}>
          <div className={styles.authorContainer}>
            <Link href={`/r/${chapter.subreddit}`}>
              <strong>r/{chapter.subreddit}</strong>
            </Link>
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
        <h2 className={styles.title}>{chapter.title}</h2>
        <ChapterTimeMetadata
          createdAtUtc={chapter.createdAtUtc}
          syncedAtUtc={chapter.syncedAtUtc}
        />
      </div>
    </PageHeader>
  );
}
