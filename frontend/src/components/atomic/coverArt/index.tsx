import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagnifyingGlassPlus } from "@fortawesome/free-solid-svg-icons";
import styles from "./coverArt.module.css";

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
  return (
    <div className={`${styles.coverArtContainer} ${className ?? ""}`}>
      <img
        className={styles.coverArt}
        src={url}
        alt={`${chapterTitle}'s cover art`}
      />
      <div className={styles.backdrop}>
        <FontAwesomeIcon icon={faMagnifyingGlassPlus} size="xl" />
      </div>
    </div>
  );
}
