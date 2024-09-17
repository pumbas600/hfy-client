import styles from "./pageLayout.module.css";
import Aside from "../aside";

export * from "./regions";

export interface PageLayoutProps {
  children?: React.ReactNode;
}

export default function PageLayout({ children }: PageLayoutProps) {
  return (
    <div className={styles.pageLayout}>
      {children}
      <div className={styles.aside}>
        <Aside />
      </div>
      <footer className={styles.footer}>
        <div className={styles.content}>
          <p>Footer</p>
        </div>
      </footer>
    </div>
  );
}
