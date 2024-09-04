import RelativeTime from "../times/relativeTime";
import styles from "./chapterTimeMetadata.module.css";

export interface ChapterTimeMetadataProps {
  createdAtUtc: string;
  syncedAtUtc: string;
  className?: string;
}

export default function ChapterTimeMetadata({
  createdAtUtc,
  syncedAtUtc,
  className,
}: ChapterTimeMetadataProps) {
  return (
    <p className={`${styles.timeMetadata} ${className}`}>
      Posted <RelativeTime dateTimeUtc={createdAtUtc} /> • Synced{" "}
      <RelativeTime dateTimeUtc={syncedAtUtc} />
    </p>
  );
}
