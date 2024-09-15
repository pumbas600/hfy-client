import PageLayout from "@/components/layout/pageLayout";
import { FullChapter } from "@/types/chapter";
import React from "react";
import styles from "./chapterLayout.module.css";
import { IconButton, UnderlinedLink } from "@/components/atomic";
import BackButton from "@/components/buttons/backButton";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";
import LabelContainer from "@/components/layout/labelContainer";
import UpvoteLabel from "@/components/labels/upvoteLabel";
import NsfwLabel from "@/components/labels/nsfwLabel";
import CoverArt from "@/components/images/coverArt";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";

export interface ChapterLayoutProps {
  children?: React.ReactNode;
  chapter: FullChapter;
}

export default function ChapterLayout({
  children,
  chapter,
}: ChapterLayoutProps) {
  const subredditLink = `/r/${chapter.subreddit}`;

  return (
    <PageLayout
      stickyStart={
        <BackButton link={subredditLink} title="Back to subreddit" />
      }
      stickyContent={
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
      }
      stickyEnd={
        <a href={chapter.redditPostLink} target="_blank">
          <IconButton icon={faReddit} title="Read on Reddit" />
        </a>
      }
      headerClassName={styles.header}
      headerContent={
        <>
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
        </>
      }
    >
      {children}
    </PageLayout>
  );
}
