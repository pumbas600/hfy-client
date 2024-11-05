import { ReactNode } from "react";
import styles from "./pageLayout.module.css";
import { Subtitle } from "@/components/atomic";
import LinkList from "@/components/composite/linkList";
import { DevelopmentLinks } from "@/config/constants";

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
          <LinkList title="Development" links={DevelopmentLinks} />
          <LinkList title="" links={{}} />
        </div>
      </footer>
    </div>
  );
}
