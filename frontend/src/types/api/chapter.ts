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

  export type ResBody = FullChapter;
  export type ReqBody = never;
}

export namespace GetNewChaptersRequest {
  export interface Params extends ChapterPaginationKey {
    subreddit: ChapterMetadata["subreddit"];
  }

  export type ResBody = PaginatedChapters;
  export type ReqBody = never;
}

export namespace GetChaptersByTitleRequest {
  export interface Params extends ChapterPaginationKey {
    subreddit: ChapterMetadata["subreddit"];
    title: string;
  }

  export type ResBody = PaginatedChapters;
  export type ReqBody = never;
}
