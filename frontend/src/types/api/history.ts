import { HistoryEntry, ReadingHistoryEntry } from "../history";

export namespace PostHistoryRequest {
  export type Params = never;
  export type ReqBody = Pick<HistoryEntry, "chapterId">;
  export type ResBody = HistoryEntry;
}

export namespace GetReadingHistoryRequest {
  export type Params = never;
  export type ReqBody = never;
  export type ResBody = ReadingHistoryEntry[];
}
