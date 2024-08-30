import Link from "next/link";

export interface ChapterButtonProps {
  chapterId?: string;
  title?: string;
  children?: React.ReactNode;
}

export default function ChapterButton({
  chapterId,
  title,
  children,
}: ChapterButtonProps) {
  if (!chapterId) {
    return (
      <button disabled title={title}>
        {children}
      </button>
    );
  }

  return (
    <Link href={`/chapters/${chapterId}`}>
      <button title={title}>{children}</button>
    </Link>
  );
}
