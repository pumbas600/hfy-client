import { SubtleLink } from "@/components/atomic";
import { Subtitle } from "@/components/atomic/typography";
import styles from "./aside.module.css";
import { Links, SupportedSubreddits } from "@/config/constants";

export interface AsideProps {
  className?: string;
}

function linksToListItems(links: Record<string, string>): React.ReactNode {
  return (
    <ul className={styles.linkList}>
      {Object.entries(links).map(([label, link]) => (
        <li key={link} className={styles.listItem}>
          <SubtleLink href={link}>{label}</SubtleLink>
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
        </nav>
      </aside>
    </div>
  );
}
