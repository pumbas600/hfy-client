import { ChapterMetadata } from "@/types/chapter";
import Link from "next/link";
import ChapterTimeMetadata from "@/components/composite/chapterTimeMetadata";
import UpvoteLabel from "../../composite/upvoteLabel";
import styles from "./chapterSummaryCard.module.css";
import CoverArt from "@/components/atomic/coverArt";
import { NsfwBadge } from "@/components/composite/badges";
import LabelContainer from "@/components/layout/labelContainer";

export interface ChapterSummaryCardProps {
  metadata: ChapterMetadata;
}

export default function ChapterSummaryCard({
  metadata,
}: ChapterSummaryCardProps) {
  return (
    // Purposely not using a Next.js link here to avoid prefetching when hovering
    <a href={`/chapters/${metadata.id}`} className={styles.cardLink}>
      <div className={styles.card}>
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
          <LabelContainer>
            <UpvoteLabel
              upvotes={metadata.upvotes}
              downvotes={metadata.downvotes}
            />
            {metadata.isNsfw && <NsfwBadge />}
          </LabelContainer>
        </div>
        {metadata.coverArtUrl && (
          <CoverArt url={metadata.coverArtUrl} chapterTitle={metadata.title} />
        )}
      </div>
    </a>
  );
}
