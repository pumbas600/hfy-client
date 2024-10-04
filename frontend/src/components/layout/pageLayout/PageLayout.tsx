import { ReactNode } from "react";
import styles from "./pageLayout.module.css";

export interface PageLayoutProps {
  children?: ReactNode;
}

export default function PageLayout({ children }: PageLayoutProps) {
  return (
    <div className={styles.pageLayout}>
      {children}
      <footer className={styles.footer}>
        <div className={styles.content}>
          <p>Footer</p>
        </div>
      </footer>
    </div>
  );
}
