import { FullChapter } from "@/types/chapter";
import ChapterButton from "@/components/buttons/chapterButton";

export interface ChapterButtonsProps {
  links: Pick<
    FullChapter,
    "firstChapterId" | "previousChapterId" | "nextChapterId"
  >;
  hideFirstLink?: boolean;
}

export default function ChapterButtons({
  links,
  hideFirstLink = false,
}: ChapterButtonsProps) {
  return (
    <div>
      {!hideFirstLink && (
        <ChapterButton chapterId={links.firstChapterId}>First</ChapterButton>
      )}
      <ChapterButton chapterId={links.previousChapterId}>
        Previous
      </ChapterButton>
      <ChapterButton chapterId={links.nextChapterId}>Next</ChapterButton>
    </div>
  );
}
