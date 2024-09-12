import { FullChapter } from "@/types/chapter";
import PageHeader from "../pageHeader";
import styles from "./chapterHeader.module.css";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";
import { UnderlinedLink } from "@/components/atomic/link";
import CoverArt from "@/components/images/coverArt";
import UpvoteLabel from "@/components/labels/upvoteLabel";
import NsfwLabel from "@/components/labels/nsfwLabel";
import LabelContainer from "../labelContainer";

export interface ChapterHeaderProps {
  chapter: FullChapter;
}

export default function ChapterHeader({ chapter }: ChapterHeaderProps) {
  const subredditLink = `/r/${chapter.subreddit}`;

  return (
    <PageHeader
      popBack
      className={styles.chapterHeader}
      redditLink={chapter.redditPostLink}
      redditLinkTitle="Read on Reddit"
    >
      {chapter.coverArtUrl && (
        <CoverArt url={chapter.coverArtUrl} chapterTitle={chapter.title} />
      )}
      <div>
        <div className={styles.authorContainer}>
          <UnderlinedLink href={subredditLink} className={styles.subreddit}>
            r/{chapter.subreddit}
          </UnderlinedLink>
          <UnderlinedLink href={chapter.redditAuthorLink}>
            u/{chapter.author}
          </UnderlinedLink>
        </div>
        <h2 className={styles.title}>{chapter.title}</h2>
        <ChapterTimeMetadata
          className={styles.timeMetadata}
          createdAtUtc={chapter.createdAtUtc}
          syncedAtUtc={chapter.syncedAtUtc}
        />
        <LabelContainer>
          <UpvoteLabel
            upvotes={chapter.upvotes}
            downvotes={chapter.downvotes}
          />
          {chapter.isNsfw && <NsfwLabel />}
        </LabelContainer>
      </div>
    </PageHeader>
  );
}
