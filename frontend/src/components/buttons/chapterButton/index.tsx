import Link from "next/link";

export interface ChapterButtonProps {
  chapterId: string | null;
  tooltip?: string;
  children?: React.ReactNode;
}

export default function ChapterButton({
  chapterId,
  tooltip,
  children,
}: ChapterButtonProps) {
  if (!chapterId) {
    return (
      <button disabled title={tooltip}>
        {children}
      </button>
    );
  }

  return (
    <Link href={`/chapters/${chapterId}`}>
      <button title={tooltip}>{children}</button>
    </Link>
  );
}
