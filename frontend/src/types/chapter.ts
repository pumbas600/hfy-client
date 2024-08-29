export interface FullChapter {
  id: string;
  author: string;
  subreddit: string;
  title: string;
  textHtml: string;
  isNsfw: boolean;
  createdAtUtc: string;
  editedAtUtc: string;
  processedAtUtc: string;
  nextChapterId?: string;
  previousChapterId?: string;
  firstChapterId?: string;
}
