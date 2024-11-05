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
              Lorem ipsum dolor sit amet. Et impedit molestias et deleniti dolor
              eos nulla necessitatibus. Sit Quis repudiandae aut quia
              repellendus et recusandae dolore sed deserunt maxime est cumque
              sapiente est nostrum atque rem sunt voluptatem. Ut totam quam sit
              optio error sed quas amet cum architecto repudiandae.{" "}
              <Link href="/about" variant="subtle">
                Learn more
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
            © {year} - Copyright {config.title}
          </p>
          <ul className={styles.iconList}>
            <li>
              <Link
                href={config.discordInviteUrl}
                variant="iconButton"
                type="ghost"
                icon={faDiscord}
                title={`${config.title} Discord`}
                size="lg"
              />
            </li>
            <li>
              <Link
                href={config.githubUrl}
                variant="iconButton"
                type="ghost"
                icon={faGithub}
                title={`${config.title} GitHub`}
                size="lg"
              />
            </li>
          </ul>
        </div>
      </div>
    </footer>
  );
}
