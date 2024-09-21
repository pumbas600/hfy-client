import {
  Aside,
  PageLayout,
  Header,
  Main,
  Sticky,
} from "@/components/layout/pageLayout";
import { FullChapter } from "@/types/chapter";
import React from "react";
import styles from "./chapterLayout.module.css";
import { UnderlinedLink } from "@/components/atomic";
import BackButton from "@/components/composite/backButton";
import LabelContainer from "@/components/layout/labelContainer";
import UpvoteLabel from "@/components/composite/upvoteLabel";
import { NsfwBadge } from "@/components/composite/badges";
import CoverArt from "@/components/atomic/coverArt";
import ChapterTimeMetadata from "@/components/composite/chapterTimeMetadata";
import RedditLink from "@/components/composite/redditLink";

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
    <PageLayout>
      <Sticky
        start={<BackButton link={subredditLink} title="Back to subreddit" />}
        end={
          <RedditLink href={chapter.redditPostLink} title="Read on Reddit" />
        }
      >
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
      </Sticky>
      <Header className={styles.header}>
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
            {chapter.isNsfw && <NsfwBadge />}
          </LabelContainer>
        </div>
      </Header>
      <Aside />
      <Main>{children}</Main>
    </PageLayout>
  );
}
