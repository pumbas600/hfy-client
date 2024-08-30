import Container from "@/components/container";
import styles from "./pageHeader.module.css";

export interface PageHeaderProps {
  children?: React.ReactNode;
}

export default function PageHeader({ children }: PageHeaderProps) {
  return (
    <header className={styles.pageHeader}>
      <Container>{children}</Container>
    </header>
  );
}
