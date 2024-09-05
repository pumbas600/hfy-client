import { Pagination } from "./pagination";

export interface ChapterMetadata {
  id: string;
  author: string;
  subreddit: string;
  title: string;
  isNsfw: boolean;
  upvotes: number;
  downvotes: number;
  redditAuthorLink: string;
  coverArtUrl: string | null;
  createdAtUtc: string;
  editedAtUtc: string;
  syncedAtUtc: string;
}

export interface FullChapter extends ChapterMetadata {
  textHtml: string;
  redditPostLink: string;
  nextChapterId: string | null;
  previousChapterId: string | null;
  firstChapterId: string | null;
}

export interface ChapterPaginationKey {
  lastCreatedAtUtc: string;
  lastPostId: string;
}

export type PaginatedChapters = Pagination<
  ChapterPaginationKey,
  ChapterMetadata
>;
