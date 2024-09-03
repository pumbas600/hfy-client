export interface ChapterMetadata {
  id: string;
  author: string;
  subreddit: string;
  title: string;
  isNsfw: boolean;
  upvotes: number;
  downvotes: number;
  createdAtUtc: string;
  editedAtUtc: string;
  syncedAtUtc: string;
}

export interface FullChapter extends ChapterMetadata {
  textHtml: string;
  redditPostLink: string;
  redditAuthorLink: string;
  nextChapterId: string | null;
  previousChapterId: string | null;
  firstChapterId: string | null;
}

export interface ChapterPaginationKey {
  lastCreatedAtUtc: string;
  lastPostId: string;
}
