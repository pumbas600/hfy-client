import Link from "next/link";

export interface ChapterButtonProps {
  chapterId: string | null;
  tooltip?: string;
  className?: string;
  children?: React.ReactNode;
}

export default function ChapterButton({
  chapterId,
  tooltip,
  className,
  children,
}: ChapterButtonProps) {
  if (!chapterId) {
    return (
      <button disabled title={tooltip} className={className}>
        {children}
      </button>
    );
  }

  return (
    <Link href={`/chapters/${chapterId}`} className={className}>
      <button title={tooltip}>{children}</button>
    </Link>
  );
}
