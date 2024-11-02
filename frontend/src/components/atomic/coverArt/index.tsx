import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagnifyingGlassPlus } from "@fortawesome/free-solid-svg-icons";
import styles from "./coverArt.module.css";
import { useRef, useState } from "react";
import Modal from "../modal";

export interface CoverArtProps {
  url: string;
  chapterTitle: string;
  className?: string;
}

export default function CoverArt({
  url,
  chapterTitle,
  className,
}: CoverArtProps) {
  const modalRef = useRef<HTMLDialogElement>(null);
  const [isExpanded, setIsExpanded] = useState(false);

  const handleExpand = (): void => {
    // setIsExpanded(true);
    modalRef.current?.showModal();
  };

  const handleCollapse = (): void => {
    setIsExpanded(false);
  };

  return (
    <>
      <div
        aria-label="Expand cover art"
        className={`${styles.coverArtContainer} ${className ?? ""}`}
        onClick={handleExpand}
      >
        <img
          className={styles.coverArt}
          src={url}
          alt={`${chapterTitle}'s cover art`}
        />
        <div className={styles.backdrop}>
          <FontAwesomeIcon icon={faMagnifyingGlassPlus} size="xl" />
        </div>
      </div>
      <Modal ref={modalRef} isOpen={isExpanded} onClose={handleCollapse}>
        Hi
      </Modal>
    </>
  );
}
