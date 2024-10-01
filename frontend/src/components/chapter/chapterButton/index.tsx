import { Link } from "@/components/atomic";

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
    <Link
      variant="button"
      href={`/chapters/${chapterId}`}
      className={className}
      title={tooltip}
    >
      {children}
    </Link>
  );
}
