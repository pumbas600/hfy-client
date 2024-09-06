import { ChapterMetadata } from "@/types/chapter";
import Link from "next/link";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";
import Upvotes from "../../upvotes";
import styles from "./chapterSummaryCard.module.css";
import CoverArt from "@/components/images/coverArt";

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
          <div className={styles.authorContainer}>
            <p>u/{metadata.author}</p>
            <ChapterTimeMetadata
              createdAtUtc={metadata.createdAtUtc}
              syncedAtUtc={metadata.syncedAtUtc}
              className={styles.subtitle}
            />
          </div>
          <h4>{metadata.title}</h4>
          <Upvotes upvotes={metadata.upvotes} downvotes={metadata.downvotes} />
        </div>
        {metadata.coverArtUrl && (
          <CoverArt url={metadata.coverArtUrl} chapterTitle={metadata.title} />
        )}
      </article>
    </Link>
  );
}
