import { ReactNode } from "react";
import styles from "./pageLayout.module.css";
import { Subtitle } from "@/components/atomic";
import LinkList from "@/components/composite/linkList";
import { DevelopmentLinks } from "@/config/constants";
import Footer from "./footer";

export interface PageLayoutProps {
  children?: ReactNode;
  className?: string;
}

export default function PageLayout({ children, className }: PageLayoutProps) {
  return (
    <div className={`${styles.pageLayout} ${className ?? ""}`}>
      {children}
      <Footer />
    </div>
  );
}
