"use client";

import { faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { IconButton } from "@/components/atomic";
import styles from "./scrollToTopButton.module.css";

export default function ScrollToTopButton() {
  const handleScrollToTop = (): void => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  return (
    <div className={styles.scrollToTopButton}>
      <IconButton
        icon={faArrowUp}
        title="Scroll to top"
        onClick={handleScrollToTop}
      />
    </div>
  );
}
