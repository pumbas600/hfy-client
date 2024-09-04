import { ChapterMetadata } from "@/types/chapter";
import Link from "next/link";
import ChapterTimeMetadata from "@/components/chapterTimeMetadata";
import Upvotes from "./upvotes";
import { UnderlinedLink } from "@/components/links";
import styles from "./chapterSummaryCard.module.css";

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
            <UnderlinedLink href={metadata.redditAuthorLink}>
              u/{metadata.author}
            </UnderlinedLink>
            <ChapterTimeMetadata
              createdAtUtc={metadata.createdAtUtc}
              syncedAtUtc={metadata.syncedAtUtc}
            />
          </div>
          <h4>{metadata.title}</h4>
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
