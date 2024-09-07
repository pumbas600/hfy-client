import { FullChapter } from "@/types/chapter";
import PageHeader from "../pageHeader";
import styles from "./chapterHeader.module.css";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";
import { UnderlinedLink } from "@/components/links";
import CoverArt from "@/components/images/coverArt";
import Upvotes from "@/components/upvotes";
import NsfwLabel from "@/components/labels/nsfwLabel";

export interface ChapterHeaderProps {
  chapter: FullChapter;
}

export default function ChapterHeader({ chapter }: ChapterHeaderProps) {
  const subredditLink = `/r/${chapter.subreddit}`;

  return (
    <PageHeader
      className={styles.chapterHeader}
      backLink={subredditLink}
      redditLink={chapter.redditPostLink}
      redditLinkTitle="Read on Reddit"
    >
      {chapter.coverArtUrl && (
        <CoverArt url={chapter.coverArtUrl} chapterTitle={chapter.title} />
      )}
      <div>
        <div className={styles.authorContainer}>
          <UnderlinedLink href={subredditLink}>
            <strong>r/{chapter.subreddit}</strong>
          </UnderlinedLink>
          <UnderlinedLink href={chapter.redditAuthorLink}>
            u/{chapter.author}
          </UnderlinedLink>
        </div>
        <h2 className={styles.title}>{chapter.title}</h2>
        <ChapterTimeMetadata
          createdAtUtc={chapter.createdAtUtc}
          syncedAtUtc={chapter.syncedAtUtc}
        />
        <div className={styles.metadataContainer}>
          <Upvotes upvotes={chapter.upvotes} downvotes={chapter.downvotes} />
          {chapter.isNsfw && <NsfwLabel />}
        </div>
      </div>
    </PageHeader>
  );
}
