import Container, { ContainerProps } from "@/components/atomic/container";
import styles from "./pageHeader.module.css";

export interface PageHeaderProps extends ContainerProps {
  popBack?: boolean;
  navStart?: React.ReactNode;
  navContent?: React.ReactNode;
  navEnd?: React.ReactNode;
  backLink?: string;
  backTitle?: string;
  redditLink?: string;
  redditLinkTitle?: string;
}

export default function PageHeader({
  popBack = false,
  navStart,
  navContent,
  navEnd,
  backLink,
  backTitle,
  redditLink,
  redditLinkTitle = "View on Reddit",
  ...containerProps
}: PageHeaderProps) {
  return (
    <>
      <nav className={styles.navBar}>
        <Container className={styles.navContainer} noBlockPadding>
          {navStart ?? <div />}
          {navContent ?? <div />}
          {navEnd ?? <div />}
        </Container>
      </nav>
      <header className={styles.pageHeader}>
        <Container noBlockPadding {...containerProps} />
      </header>
    </>
  );
}
