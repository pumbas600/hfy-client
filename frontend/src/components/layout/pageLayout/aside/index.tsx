import { Link } from "@/components/atomic";
import { Subtitle } from "@/components/atomic/typography";
import styles from "./aside.module.css";
import {
  DevelopmentLinks,
  Links,
  SupportedSubreddits,
} from "@/config/constants";

export interface AsideProps {
  className?: string;
}

function linksToListItems(links: Record<string, string>): React.ReactNode {
  return (
    <ul className={styles.linkList}>
      {Object.entries(links).map(([label, link]) => (
        <li key={label + link} className={styles.listItem}>
          <Link variant="subtle" href={link}>
            {label}
          </Link>
        </li>
      ))}
    </ul>
  );
}

export default function Aside({ className }: AsideProps) {
  return (
    <div className={styles.container}>
      <aside className={`${styles.aside} ${className ?? ""}`}>
        <nav className={styles.nav}>
          <Subtitle>Subreddits</Subtitle>
          {linksToListItems(SupportedSubreddits)}
          <Subtitle>Info</Subtitle>
          {linksToListItems(Links)}
          <Subtitle>Development</Subtitle>
          {linksToListItems(DevelopmentLinks)}
        </nav>
      </aside>
    </div>
  );
}
