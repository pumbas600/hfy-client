import { FullChapter } from "@/types/chapter";
import ChapterButton from "@/components/buttons/chapterButton";
import styles from "./chapterButtons.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAnglesLeft } from "@fortawesome/free-solid-svg-icons/faAnglesLeft";
import { faAngleLeft } from "@fortawesome/free-solid-svg-icons";
import { faAngleRight } from "@fortawesome/free-solid-svg-icons/faAngleRight";

export interface ChapterButtonsProps {
  chapter: FullChapter;
  hideFirstLink?: boolean;
}

export default function ChapterButtons({
  chapter,
  hideFirstLink = false,
}: ChapterButtonsProps) {
  const isFirst = chapter.firstChapterId === chapter.id;

  if (isFirst && chapter.nextChapterId === null) {
    return;
  }

  const previousChapterTooltip =
    chapter.previousChapterId === null && !isFirst
      ? "The previous chapter cannot be determined"
      : undefined;

  return (
    <div className={styles.buttonGroup}>
      <div className={styles.previousButtons}>
        {!(hideFirstLink || isFirst) && (
          <ChapterButton chapterId={chapter.firstChapterId}>
            <FontAwesomeIcon icon={faAnglesLeft} />
            First
          </ChapterButton>
        )}
        <ChapterButton
          chapterId={chapter.previousChapterId}
          tooltip={previousChapterTooltip}
        >
          <FontAwesomeIcon icon={faAngleLeft} />
          Previous
        </ChapterButton>
      </div>
      <ChapterButton chapterId={chapter.nextChapterId}>
        Next
        <FontAwesomeIcon icon={faAngleRight} />
      </ChapterButton>
    </div>
  );
}
