import { Subtitle, Link } from "@/components/atomic";
import styles from "./linkList.module.css";

interface LinkListProps {
  title: string;
  links: Record<string, string>;
}

export default function LinkList({ title, links }: LinkListProps) {
  return (
    <div>
      <Subtitle>{title}</Subtitle>
      <ul className={styles.linkList}>
        {Object.entries(links).map(([label, link]) => (
          <li key={label + link} className={styles.listItem}>
            <Link variant="subtle" href={link}>
              {label}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
}
