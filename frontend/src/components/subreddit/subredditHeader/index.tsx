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
      navContent={<ChapterSearchInput subreddit={subreddit.name} />}
      className={styles.wrapper}
    >
      <img src={subreddit.iconUrl} alt={`${subreddit.name}'s icon`} />
      <div>
        <h3 className={styles.subredditName}>r/{subreddit.name}</h3>
        <h2>{subreddit.title}</h2>
      </div>
    </PageHeader>
  );
}
