import { RelativeTime } from "@/components/atomic";
import styles from "./chapterTimeMetadata.module.css";
import { cx } from "@/util/classNames";

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
    <p className={cx(styles.timeMetadata, className)}>
      Posted <RelativeTime dateTimeUtc={createdAtUtc} /> • Synced{" "}
      <RelativeTime dateTimeUtc={syncedAtUtc} />
    </p>
  );
}
