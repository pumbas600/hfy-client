import { NoSsr, Subtitle } from "@/components/atomic";
import styles from "./aside.module.css";
import {
  DevelopmentLinks,
  Links,
  SupportedSubreddits,
} from "@/config/constants";
import { getSelf } from "@/lib/getSelf";
import LinkList from "@/components/composite/linkList";

export interface AsideProps {
  className?: string;
}

export default function Aside({ className }: AsideProps) {
  return (
    <div className={styles.container}>
      <aside className={`${styles.aside} ${className ?? ""}`}>
        <nav className={styles.nav}>
          {getSelf() && (
            <NoSsr>
              <LinkList title="Subreddits" links={SupportedSubreddits} />
            </NoSsr>
          )}
          <LinkList title="Info" links={Links} />
          <LinkList title="Development" links={DevelopmentLinks} />
        </nav>
      </aside>
    </div>
  );
}
