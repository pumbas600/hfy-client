import { PageLayout } from "@/components/layout/pageLayout";
import { ReactNode } from "react";
import styles from "./primaryLayout.module.css";

interface PrimaryLayoutProps {
  children?: ReactNode;
}

export default function PrimaryLayout({ children }: PrimaryLayoutProps) {
  return <PageLayout className={styles.primaryLayout}>{children}</PageLayout>;
}
