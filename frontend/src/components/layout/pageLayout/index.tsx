import styles from "./pageLayout.module.css";

export default function PageLayout() {
  return (
    <div className={styles.pageLayout}>
      <div className={styles.sticky}>
        <div className={styles.content}>Sticky header</div>
      </div>
      <header className={styles.header}>
        <div className={styles.content}>
          <h1>Page</h1>
        </div>
      </header>
      <aside className={styles.aside}>
        <h4>Aside</h4>
      </aside>
      <main className={styles.main}>
        <p>Content goes here</p>
      </main>
      <footer className={styles.footer}>
        <div className={styles.content}>
          <p>Footer</p>
        </div>
      </footer>
    </div>
  );
}
