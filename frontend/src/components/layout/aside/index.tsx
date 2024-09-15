import { Subtitle } from "@/components/typography";

export interface AsideProps {
  className?: string;
}

export default function Aside({ className }: AsideProps) {
  return (
    <aside className={className}>
      <nav>
        <Subtitle>Subreddits</Subtitle>

        <Subtitle>Links</Subtitle>
      </nav>
    </aside>
  );
}
