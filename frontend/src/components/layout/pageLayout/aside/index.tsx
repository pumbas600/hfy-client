import styles from "./aside.module.css";
import { DevelopmentLinks, Links } from "@/config/constants";
import LinkList from "@/components/composite/linkList";

export interface AsideProps {
  className?: string;
}

export default function Aside({ className }: AsideProps) {
  return (
    <div className={styles.container}>
      <aside className={`${styles.aside} ${className ?? ""}`}>
        <nav className={styles.nav}>
          <LinkList title="Info" links={Links} />
          <LinkList title="Development" links={DevelopmentLinks} />
        </nav>
      </aside>
    </div>
  );
}
