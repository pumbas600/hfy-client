import { ChapterMetadata, ChapterPaginationKey, FullChapter } from "../chapter";
import { Pagination } from "../pagination";

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

  export type ResBody = Pagination<ChapterPaginationKey, ChapterMetadata>;
  export type ReqBody = never;
}
