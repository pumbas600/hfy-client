import {
  Aside,
  PageLayout,
  Header,
  Main,
  Sticky,
} from "@/components/layout/pageLayout";
import { Subreddit } from "@/types/subreddit";
import styles from "./subredditLayout.module.css";
import ChapterSearchInput from "../chapterSearchInput";
import RedditLink from "@/components/composite/redditLink";
import SelfProfile from "@/components/composite/selfProfile";
import { User } from "@/types/user";

export interface SubredditLayoutProps {
  children?: React.ReactNode;
  subreddit: Subreddit;
  self: User;
}

export default function SubredditLayout({
  children,
  subreddit,
  self,
}: SubredditLayoutProps) {
  return (
    <PageLayout>
      <Sticky
        start={
          <img
            src={subreddit.iconUrl}
            alt={`${subreddit.name}'s icon`}
            title={subreddit.name}
            className={styles.icon}
            style={{ backgroundColor: subreddit.iconBackgroundColor }}
          />
        }
        end={[
          <RedditLink
            key="reddit"
            href={subreddit.redditLink}
            title="View on Reddit"
          />,
          <SelfProfile key="profile" user={self} />,
        ]}
      >
        <ChapterSearchInput
          subreddit={subreddit.name}
          className={styles.searchInput}
        />
      </Sticky>
      <Header className={styles.header}>
        <div>
          <h3 className={styles.name}>r/{subreddit.name}</h3>
          <h2 className={styles.title}>{subreddit.title}</h2>
        </div>
        <p className={styles.description}>{subreddit.description}</p>
      </Header>
      <Aside />
      <Main noInlinePadding>{children}</Main>
    </PageLayout>
  );
}
