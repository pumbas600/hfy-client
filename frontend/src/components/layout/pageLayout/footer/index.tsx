import LinkList from "@/components/composite/linkList";
import { DevelopmentLinks } from "@/config/constants";
import styles from "./footer.module.css";
import layoutStyles from "../pageLayout.module.css";

export default function Footer() {
  const year = new Date().getFullYear();

  return (
    <footer className={styles.footer}>
      <div className={layoutStyles.content}>
        <LinkList title="Development" links={DevelopmentLinks} />
        <LinkList
          title="Policy"
          links={{ "Privacy Policy": "/privacy-policy" }}
        />
        <hr />
        <p>{year}</p>
      </div>
    </footer>
  );
}
