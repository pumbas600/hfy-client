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
      navStart={
        <img
          src={subreddit.iconUrl}
          alt={`${subreddit.name}'s icon`}
          className={styles.icon}
        />
      }
      navContent={<ChapterSearchInput subreddit={subreddit.name} />}
      className={styles.wrapper}
    >
      <div>
        <h3 className={styles.name}>r/{subreddit.name}</h3>
        <h2 className={styles.title}>{subreddit.title}</h2>
      </div>
      <p className={styles.description}>{subreddit.description}</p>
    </PageHeader>
  );
}
