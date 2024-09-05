import Container, { ContainerProps } from "@/components/container";
import styles from "./pageHeader.module.css";
import Link from "next/link";
import IconButton from "@/components/buttons/iconButton";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { faReddit } from "@fortawesome/free-brands-svg-icons";

export interface PageHeaderProps extends ContainerProps {
  backLink?: string;
  redditLink?: string;
  redditLinkTitle?: string;
}

export default function PageHeader({
  backLink,
  redditLink,
  redditLinkTitle,
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
                <IconButton icon={faArrowLeft} title="Back" />
              </Link>
            )}
            {redditLink && (
              <a href={redditLink}>
                <IconButton
                  icon={faReddit}
                  title={redditLinkTitle ?? "View on Reddit"}
                />
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
