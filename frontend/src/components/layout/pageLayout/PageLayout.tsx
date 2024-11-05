import { ReactNode } from "react";
import styles from "./pageLayout.module.css";
import { cx } from "@/util/classNames";
export interface PageLayoutProps {
  children?: ReactNode;
  className?: string;
}

export default function PageLayout({ children, className }: PageLayoutProps) {
  return <div className={cx(styles.pageLayout, className)}>{children}</div>;
}
