import {
  ChapterMetadata,
  ChapterPaginationKey,
  FullChapter,
  PaginatedChapters,
} from "../chapter";

export namespace GetChapterRequest {
  export interface Params {
    chapterId: ChapterMetadata["id"];
  }

  export type ReqBody = never;
  export type ResBody = FullChapter;
}

export namespace GetNewChaptersRequest {
  export interface Params extends ChapterPaginationKey {
    subreddit: ChapterMetadata["subreddit"];
  }

  export type ReqBody = never;
  export type ResBody = PaginatedChapters;
}

export namespace GetChaptersByTitleRequest {
  export interface Params extends ChapterPaginationKey {
    subreddit: ChapterMetadata["subreddit"];
    title: string;
  }

  export type ReqBody = never;
  export type ResBody = PaginatedChapters;
}
