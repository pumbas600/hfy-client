import Container, { ContainerProps } from "@/components/atomic/container";
import styles from "./pageHeader.module.css";

export interface PageHeaderProps extends ContainerProps {
  popBack?: boolean;
  navStart?: React.ReactNode;
  navContent?: React.ReactNode;
  navEnd?: React.ReactNode;
  navClassName?: string;
}

export default function PageHeader({
  popBack = false,
  navStart,
  navContent,
  navEnd,
  navClassName,
  ...containerProps
}: PageHeaderProps) {
  return (
    <>
      <nav className={`${styles.navBar} ${navClassName ?? ""}`}>
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
