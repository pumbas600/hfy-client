import Container, { ContainerProps } from "@/components/atomic/container";
import styles from "./pageHeader.module.css";
import IconButton from "@/components/atomic/iconButton";
import { faReddit } from "@fortawesome/free-brands-svg-icons";
import BackButton from "@/components/buttons/backButton";

export interface PageHeaderProps extends ContainerProps {
  popBack?: boolean;
  navContent?: React.ReactNode;
  backLink?: string;
  backTitle?: string;
  redditLink?: string;
  redditLinkTitle?: string;
}

export default function PageHeader({
  popBack = false,
  navContent,
  backLink,
  backTitle,
  redditLink,
  redditLinkTitle = "View on Reddit",
  ...containerProps
}: PageHeaderProps) {
  const hasNavBar =
    popBack === false || backLink !== undefined || redditLink !== undefined;

  return (
    <>
      {hasNavBar && (
        <nav className={styles.navBar}>
          <Container className={styles.navContainer}>
            {(backLink || popBack) && (
              <BackButton link={backLink} title={backTitle} />
            )}
            {navContent}
            {redditLink && (
              <a href={redditLink}>
                <IconButton icon={faReddit} title={redditLinkTitle} />
              </a>
            )}
          </Container>
        </nav>
      )}
      <header className={styles.pageHeader}>
        <Container {...containerProps} />
      </header>
    </>
  );
}
