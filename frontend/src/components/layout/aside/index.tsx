import { SubtleLink } from "@/components/atomic";
import { Subtitle } from "@/components/atomic/typography";
import styles from "./aside.module.css";

export interface AsideProps {
  className?: string;
}

function linksToListItems(links: Record<string, string>): React.ReactNode {
  return (
    <ul className={styles.linkList}>
      {Object.entries(links).map(([label, link]) => (
        <li key={link}>
          <SubtleLink href={link}>{label}</SubtleLink>
        </li>
      ))}
    </ul>
  );
}

export default function Aside({ className }: AsideProps) {
  const subredditLinks = {
    "r/HFY": "/r/HFY",
  } as const;

  return (
    <aside className={className}>
      <nav className={styles.nav}>
        <Subtitle>Subreddits</Subtitle>
        {linksToListItems(subredditLinks)}
        <Subtitle>Links</Subtitle>
      </nav>
    </aside>
  );
}
