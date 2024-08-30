import styles from "./chapterText.module.css";

export interface ChapterTextProps {
  textHtml: string;
}

export default function ChapterText({ textHtml }: ChapterTextProps) {
  return (
    <main
      className={styles.chapterContainer}
      dangerouslySetInnerHTML={{ __html: textHtml }}
    />
  );
}
