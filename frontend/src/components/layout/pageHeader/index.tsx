import Container, { ContainerProps } from "@/components/container";
import styles from "./pageHeader.module.css";
import Link from "next/link";
import IconButton from "@/components/buttons/iconButton";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { faReddit } from "@fortawesome/free-brands-svg-icons";

export interface PageHeaderProps extends ContainerProps {
  backLink?: string;
  backTitle?: string;
  redditLink?: string;
  redditLinkTitle?: string;
}

export default function PageHeader({
  backLink,
  backTitle = "Back",
  redditLink,
  redditLinkTitle = "View on Reddit",
  ...containerProps
}: PageHeaderProps) {
  const hasNavBar = backLink !== undefined || redditLink !== undefined;

  return (
    <>
      {hasNavBar && (
        <nav className={styles.navBar}>
          <Container className={styles.navContainer}>
            {backLink && (
              <Link href={backLink}>
                <IconButton icon={faArrowLeft} title={backTitle} />
              </Link>
            )}
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
