import { ChapterMetadata } from "./chapter";

export interface HistoryEntry {
  id: number;
  chapterId: string;
  readAtUtc: string;
}

export interface ReadingHistoryEntry {
  id: number;
  chapterMetadata: ChapterMetadata;
  nextChapterId: string | null;
  readAtUtc: string;
}
