import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagnifyingGlassPlus } from "@fortawesome/free-solid-svg-icons";
import styles from "./coverArt.module.css";
import Modal from "../modal";
import useModal from "@/hooks/useModal";
import { cx } from "@/util/classNames";
import IconButton from "../iconButton";
import { faClose } from "@fortawesome/free-solid-svg-icons/faClose";

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
  const { modalRef, open, close } = useModal();

  const altText = `${chapterTitle}'s cover art`;

  return (
    <>
      <div
        aria-label="Expand cover art"
        className={cx(styles.coverArtContainer, className)}
        onClick={open}
      >
        <img className={styles.coverArt} src={url} alt={altText} />
        <div className={styles.backdrop}>
          <FontAwesomeIcon icon={faMagnifyingGlassPlus} size="xl" />
        </div>
      </div>
      <Modal ref={modalRef} onClose={close} className={styles.expandedModal}>
        <IconButton
          autoFocus
          icon={faClose}
          title="Close modal"
          onClick={close}
        />
        <img src={url} alt={altText} />
      </Modal>
    </>
  );
}
