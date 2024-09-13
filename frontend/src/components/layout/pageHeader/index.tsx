import Container, { ContainerProps } from "@/components/atomic/container";
import styles from "./pageHeader.module.css";
import IconButton from "@/components/atomic/iconButton";
import { faReddit } from "@fortawesome/free-brands-svg-icons";
import BackButton from "@/components/buttons/backButton";

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
          {(backLink || popBack) && (
            <BackButton link={backLink} title={backTitle} />
          )}
          {navContent ?? <div />}
          {redditLink && (
            <a href={redditLink}>
              <IconButton icon={faReddit} title={redditLinkTitle} />
            </a>
          )}
          {navEnd ?? <div />}
        </Container>
      </nav>
      <header className={styles.pageHeader}>
        <Container noBlockPadding {...containerProps} />
      </header>
    </>
  );
}
