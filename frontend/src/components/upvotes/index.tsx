import { ChapterMetadata } from "@/types/chapter";
import styles from "./upvotes.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHeart } from "@fortawesome/free-regular-svg-icons/faHeart";

export interface UpvotesProps {
  upvotes: ChapterMetadata["upvotes"];
  downvotes: ChapterMetadata["downvotes"];
}

export default function Upvotes({ upvotes, downvotes }: UpvotesProps) {
  const totalVotes = upvotes - downvotes;

  const formatVotes = (votes: number): string => {
    if (votes > 1000) {
      return `${(votes / 1000).toFixed(1)}k`;
    }

    return votes.toString();
  };

  return (
    <div className={styles.upvotes} title={`${totalVotes} upvotes`}>
      <FontAwesomeIcon icon={faHeart} />
      <p>{formatVotes(upvotes - downvotes)}</p>
    </div>
  );
}
