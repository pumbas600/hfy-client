import LinkList from "@/components/composite/linkList";
import { DevelopmentLinks } from "@/config/constants";
import styles from "./footer.module.css";
import layoutStyles from "../pageLayout.module.css";
import config from "@/config";
import { Link, Subtitle } from "@/components/atomic";
import { faDiscord } from "@fortawesome/free-brands-svg-icons/faDiscord";
import { faGithub } from "@fortawesome/free-brands-svg-icons/faGithub";

export default function Footer() {
  const year = new Date().getFullYear();

  return (
    <footer className={styles.footer}>
      <div className={layoutStyles.content}>
        <div className={styles.footerContent}>
          <div>
            <Subtitle>About</Subtitle>
            <p className={styles.footerParagraph}>
              {config.title} is a free, open-source client for Reddit dedicated
              to the r/HFY subreddit. Designed for reading stories, it contains
              features to improve your reading experience.{" "}
              <Link href="/about" variant="subtle">
                Learn more here
              </Link>
              .
            </p>
          </div>
          <LinkList title="Development" links={DevelopmentLinks} />
          <LinkList
            title="Resources"
            links={{ "Privacy policy": "/privacy-policy" }}
          />
        </div>
        <hr />
        <div className={styles.footerMetadata}>
          <p className={styles.footerParagraph}>
            © {year} &ndash; Copyright {config.title}
          </p>
          <div className={styles.iconList}>
            <Link
              href={config.discordInviteUrl}
              variant="iconButton"
              type="ghost"
              icon={faDiscord}
              title={`${config.title} Discord`}
              size="lg"
            />
            <span className={styles.verticalDivider} />
            <Link
              href={config.githubUrl}
              variant="iconButton"
              type="ghost"
              icon={faGithub}
              title={`${config.title} GitHub`}
            />
          </div>
        </div>
      </div>
    </footer>
  );
}
