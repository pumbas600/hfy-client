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
import { Link } from "@/components/atomic";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";

export interface SubredditLayoutProps {
  children?: React.ReactNode;
  subreddit: Subreddit;
}

export default function SubredditLayout({
  children,
  subreddit,
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
          <Link
            variant="iconButton"
            key="reddit"
            href={subreddit.redditLink}
            title="View on Reddit"
            icon={faReddit}
          />,
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
