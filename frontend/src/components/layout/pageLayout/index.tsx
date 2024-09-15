import styles from "./pageLayout.module.css";

export interface PageLayoutProps {
  stickyStart?: React.ReactNode;
  stickyContent?: React.ReactNode;
  stickyEnd?: React.ReactNode;
  stickyClassName?: string;
  headerContent?: React.ReactNode;
  headerClassName?: string;
  mainClassName?: string;
  noMainPadding?: boolean;
  children?: React.ReactNode;
}

export default function PageLayout({
  children,
  stickyStart,
  stickyContent,
  stickyEnd,
  stickyClassName,
  headerContent,
  headerClassName,
  mainClassName,
  noMainPadding,
}: PageLayoutProps) {
  const mainPaddingClassName = noMainPadding ? "" : styles.mainPadding;

  return (
    <div className={styles.pageLayout}>
      <div className={`${styles.sticky} ${stickyClassName ?? ""}`}>
        <div className={styles.stickyContent}>
          {stickyStart ?? <div />}
          {stickyContent ?? <div />}
          {stickyEnd ?? <div />}
        </div>
      </div>
      <header className={styles.header}>
        <div className={`${styles.content} ${headerClassName ?? ""}`}>
          {headerContent}
        </div>
      </header>
      <aside className={styles.aside}>
        <nav>
          <h4>Aside</h4>
        </nav>
      </aside>
      <main
        className={`${styles.main} ${mainPaddingClassName} ${
          mainClassName ?? ""
        }`}
      >
        {children}
      </main>
      <footer className={styles.footer}>
        <div className={styles.content}>
          <p>Footer</p>
        </div>
      </footer>
    </div>
  );
}
