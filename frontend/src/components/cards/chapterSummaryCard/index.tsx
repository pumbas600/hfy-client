import { ChapterMetadata } from "@/types/chapter";
import Link from "next/link";
import styles from "./chapterSummaryCard.module.css";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";
import Upvotes from "./upvotes";

export interface ChapterSummaryCardProps {
  metadata: ChapterMetadata;
}

export default function ChapterSummaryCard({
  metadata,
}: ChapterSummaryCardProps) {
  return (
    <Link href={`/chapters/${metadata.id}`} className={styles.cardLink}>
      <article className={styles.card}>
        <div>
          <div>
            <a href={metadata.redditAuthorLink}>u/{metadata.author}</a>
          </div>
          <h4>{metadata.title}</h4>
          <ChapterTimeMetadata
            createdAtUtc={metadata.createdAtUtc}
            syncedAtUtc={metadata.syncedAtUtc}
          />
          <Upvotes upvotes={metadata.upvotes} downvotes={metadata.downvotes} />
        </div>
        {metadata.coverArtUrl && (
          <img
            src={metadata.coverArtUrl}
            alt={`${metadata.title}'s cover art`}
          />
        )}
      </article>
    </Link>
  );
}
