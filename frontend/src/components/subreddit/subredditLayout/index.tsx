import PageLayout from "@/components/layout/pageLayout";
import { Subreddit } from "@/types/subreddit";
import styles from "./subredditLayout.module.css";
import ChapterSearchInput from "../chapterSearchInput";
import RedditLink from "@/components/buttons/redditLink";

export interface SubredditLayoutProps {
  children?: React.ReactNode;
  subreddit: Subreddit;
}

export default function SubredditLayout({
  children,
  subreddit,
}: SubredditLayoutProps) {
  return (
    <PageLayout
      stickyStart={
        <img
          src={subreddit.iconUrl}
          alt={`${subreddit.name}'s icon`}
          title={subreddit.name}
          className={styles.icon}
          style={{ backgroundColor: subreddit.iconBackgroundColor }}
        />
      }
      stickyContent={
        <ChapterSearchInput
          subreddit={subreddit.name}
          className={styles.searchInput}
        />
      }
      stickyEnd={
        <RedditLink href={subreddit.redditLink} title="View on Reddit" />
      }
      headerClassName={styles.header}
      headerContent={
        <>
          <div>
            <h3 className={styles.name}>r/{subreddit.name}</h3>
            <h2 className={styles.title}>{subreddit.title}</h2>
          </div>
          <p className={styles.description}>{subreddit.description}</p>
        </>
      }
      noMainPadding
    >
      {children}
    </PageLayout>
  );
}
