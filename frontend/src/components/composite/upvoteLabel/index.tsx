import { ChapterMetadata } from "@/types/chapter";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHeart } from "@fortawesome/free-regular-svg-icons/faHeart";
import { Label } from "@/components/atomic";

export interface UpvoteLabelProps {
  upvotes: ChapterMetadata["upvotes"];
  downvotes: ChapterMetadata["downvotes"];
}

export default function UpvoteLabel({ upvotes, downvotes }: UpvoteLabelProps) {
  const totalVotes = upvotes - downvotes;

  const formatVotes = (votes: number): string => {
    if (votes > 1000) {
      return `${(votes / 1000).toFixed(1)}k`;
    }

    return votes.toString();
  };

  const plural = totalVotes === 1 ? "" : "s";

  return (
    <Label
      icon={<FontAwesomeIcon icon={faHeart} />}
      label={formatVotes(upvotes - downvotes)}
      title={`${totalVotes} upvote${plural}`}
    />
  );
}
