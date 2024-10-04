import { PageLayout } from "@/components/layout/pageLayout";
import { ReactNode } from "react";
import styles from "./loginLayout.module.css";

interface LoginLayoutProps {
  children?: ReactNode;
}

export default function LoginLayout({ children }: LoginLayoutProps) {
  return <PageLayout className={styles.loginLayout}>{children}</PageLayout>;
}
