import { ReactNode } from "react";
import styles from "./pageLayout.module.css";
import { Subtitle } from "@/components/atomic";

export interface PageLayoutProps {
  children?: ReactNode;
  className?: string;
}

export default function PageLayout({ children, className }: PageLayoutProps) {
  return (
    <div className={`${styles.pageLayout} ${className ?? ""}`}>
      {children}
      <footer className={styles.footer}>
        <div className={styles.content}>
          <Subtitle>Development</Subtitle>
        </div>
      </footer>
    </div>
  );
}
