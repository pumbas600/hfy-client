import RelativeTime from "../times/relativeTime";
import styles from "./chapterTimeMetadata.module.css";

export interface ChapterTimeMetadataProps {
  createdAtUtc: string;
  processedAtUtc: string;
}

export default function ChapterTimeMetadata({
  createdAtUtc,
  processedAtUtc,
}: ChapterTimeMetadataProps) {
  return (
    <p className={styles.timeMetadata}>
      Posted <RelativeTime dateTimeUtc={createdAtUtc} /> • Last synced{" "}
      <RelativeTime dateTimeUtc={processedAtUtc} />
    </p>
  );
}
