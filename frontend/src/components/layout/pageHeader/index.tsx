import Container, { ContainerProps } from "@/components/atomic/container";
import styles from "./pageHeader.module.css";
import Link from "next/link";
import IconButton from "@/components/atomic/iconButton";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { faReddit } from "@fortawesome/free-brands-svg-icons";
import { useRouter } from "next/navigation";
import BackButton from "@/components/buttons/backButton";

export interface PageHeaderProps extends ContainerProps {
  popBack?: boolean;
  backLink?: string;
  backTitle?: string;
  redditLink?: string;
  redditLinkTitle?: string;
}

export default function PageHeader({
  popBack = false,
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
