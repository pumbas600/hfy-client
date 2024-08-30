import { FullChapter } from "@/types/chapter";
import ChapterButton from "@/components/buttons/chapterButton";

export interface ChapterButtonsProps {
  chapter: FullChapter;
  hideFirstLink?: boolean;
}

export default function ChapterButtons({
  chapter,
  hideFirstLink = false,
}: ChapterButtonsProps) {
  const isFirst = chapter.firstChapterId === chapter.id;
  const previousChapterTooltip =
    chapter.previousChapterId === null && !isFirst
      ? "The previous chapter cannot be determined"
      : undefined;

  return (
    <div>
      {!(hideFirstLink || isFirst) && (
        <ChapterButton chapterId={chapter.firstChapterId}>First</ChapterButton>
      )}
      <ChapterButton
        chapterId={chapter.previousChapterId}
        tooltip={previousChapterTooltip}
      >
        Previous
      </ChapterButton>
      <ChapterButton chapterId={chapter.nextChapterId}>Next</ChapterButton>
    </div>
  );
}
