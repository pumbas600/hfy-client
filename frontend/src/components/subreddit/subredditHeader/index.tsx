import ChapterSearchInput from "@/components/inputs/chapterSearchInput";
import PageHeader from "@/components/layout/pageHeader";
import { Subreddit } from "@/types/subreddit";
import styles from "./subredditHeader.module.css";

export interface SubredditHeaderProps {
  subreddit: Subreddit;
}

export default function SubredditHeader({ subreddit }: SubredditHeaderProps) {
  return (
    <PageHeader
      navContent={
        <>
          <img
            src={subreddit.iconUrl}
            alt={`${subreddit.name}'s icon`}
            className={styles.subredditIcon}
          />
          <ChapterSearchInput
            subreddit={subreddit.name}
            className={styles.searchInput}
          />
        </>
      }
      className={styles.wrapper}
    >
      <div>
        <h3>r/{subreddit.name}</h3>
        <h2 className={styles.subredditTitle}>{subreddit.title}</h2>
      </div>
    </PageHeader>
  );
}
