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
    <img
      src={url}
      alt={`${chapterTitle}'s cover art`}
      className={`${styles.coverArt} ${className ?? ""}`}
    />
  );
}
