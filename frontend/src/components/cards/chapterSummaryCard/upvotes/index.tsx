import IconButton from "@/components/buttons/iconButton";
import { ChapterMetadata } from "@/types/chapter";
import { faDownLong } from "@fortawesome/free-solid-svg-icons/faDownLong";
import { faUpLong } from "@fortawesome/free-solid-svg-icons/faUpLong";
import styles from "./upvotes.module.css";

export interface UpvotesProps {
  upvotes: ChapterMetadata["upvotes"];
  downvotes: ChapterMetadata["downvotes"];
}

export default function Upvotes({ upvotes, downvotes }: UpvotesProps) {
  return (
    <div className={styles.upvotes}>
      <IconButton icon={faUpLong} title="Upvote chapter" />
      <strong>{upvotes - downvotes}</strong>
      <IconButton icon={faDownLong} title="Downvote chapter" />
    </div>
  );
}
