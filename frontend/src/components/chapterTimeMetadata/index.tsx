import RelativeTime from "../times/relativeTime";
import styles from "./chapterTimeMetadata.module.css";

export interface ChapterTimeMetadataProps {
  createdAtUtc: string;
  syncedAtUtc: string;
}

export default function ChapterTimeMetadata({
  createdAtUtc,
  syncedAtUtc,
}: ChapterTimeMetadataProps) {
  return (
    <p className={styles.timeMetadata}>
      Posted <RelativeTime dateTimeUtc={createdAtUtc} /> • Last synced{" "}
      <RelativeTime dateTimeUtc={syncedAtUtc} />
    </p>
  );
}
