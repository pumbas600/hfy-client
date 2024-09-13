import { FullChapter } from "@/types/chapter";
import PageHeader from "../pageHeader";
import styles from "./chapterHeader.module.css";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";
import { UnderlinedLink } from "@/components/atomic/link";
import CoverArt from "@/components/images/coverArt";
import UpvoteLabel from "@/components/labels/upvoteLabel";
import NsfwLabel from "@/components/labels/nsfwLabel";
import LabelContainer from "../labelContainer";
import BackButton from "@/components/buttons/backButton";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";
import { IconButton } from "@/components/atomic";

export interface ChapterHeaderProps {
  chapter: FullChapter;
}

export default function ChapterHeader({ chapter }: ChapterHeaderProps) {
  const subredditLink = `/r/${chapter.subreddit}`;

  return (
    <PageHeader
      className={styles.chapterHeader}
      navStart={<BackButton link={subredditLink} title="Back to subreddit" />}
      navContent={
        <div>
          <div className={styles.authorWrapper}>
            <UnderlinedLink href={subredditLink} className={styles.subreddit}>
              r/{chapter.subreddit}
            </UnderlinedLink>
            <UnderlinedLink
              href={chapter.redditAuthorLink}
              className={styles.author}
            >
              u/{chapter.author}
            </UnderlinedLink>
          </div>
        </div>
      }
      navEnd={
        <a href={chapter.redditPostLink}>
          <IconButton icon={faReddit} title="Read on Reddit" />
        </a>
      }
    >
      {chapter.coverArtUrl && (
        <CoverArt url={chapter.coverArtUrl} chapterTitle={chapter.title} />
      )}
      <div>
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
