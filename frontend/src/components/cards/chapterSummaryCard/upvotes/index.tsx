import IconButton from "@/components/buttons/iconButton";
import { ChapterMetadata } from "@/types/chapter";
import { faDownLong } from "@fortawesome/free-solid-svg-icons/faDownLong";
import { faUpLong } from "@fortawesome/free-solid-svg-icons/faUpLong";

export interface UpvotesProps {
  upvotes: ChapterMetadata["upvotes"];
  downvotes: ChapterMetadata["downvotes"];
}

export default function Upvotes({ upvotes, downvotes }: UpvotesProps) {
  return (
    <div>
      <IconButton icon={faUpLong} title="Upvote chapter" />
      {upvotes - downvotes}
      <IconButton icon={faDownLong} title="Downvote chapter" />
    </div>
  );
}
