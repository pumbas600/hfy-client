import { IconButton } from "@/components/atomic";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";

export interface RedditLinkProps {
  href: string;
  title: string;
}

export default function RedditLink({ href, title }: RedditLinkProps) {
  return (
    <a href={href} target="_blank">
      <IconButton icon={faReddit} title={title} />
    </a>
  );
}
