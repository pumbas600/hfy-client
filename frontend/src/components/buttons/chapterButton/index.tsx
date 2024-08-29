import Link from "next/link";

export interface ChapterButtonProps {
  chapterId?: string;
  children?: React.ReactNode;
}

export default function ChapterButton({
  chapterId,
  children,
}: ChapterButtonProps) {
  if (!chapterId) {
    return <button disabled>{children}</button>;
  }

  return (
    <Link href={`/chapter/${chapterId}`}>
      <button>{children}</button>
    </Link>
  );
}
