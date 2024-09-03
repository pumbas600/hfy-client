import { ChapterMetadata } from "@/types/chapter";
import Link from "next/link";
import styles from "./chapterSummaryCard.module.css";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";

export interface ChapterSummaryCardProps {
  metadata: ChapterMetadata;
}

export default function ChapterSummaryCard({
  metadata,
}: ChapterSummaryCardProps) {
  return (
    <Link href={`/chapters/${metadata.id}`} className={styles.cardLink}>
      <article className={styles.card}>
        <h4>{metadata.title}</h4>
        <ChapterTimeMetadata
          createdAtUtc={metadata.createdAtUtc}
          processedAtUtc={metadata.processedAtUtc}
        />
        <p>
          <button>Upvote</button>
          <span>{metadata.upvotes - metadata.downvotes}</span>
          <button>Downvote</button>
        </p>
      </article>
    </Link>
  );
}
