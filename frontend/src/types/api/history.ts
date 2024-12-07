import { HistoryEntry } from "../history";

export namespace PostHistoryRequest {
  export type Params = never;
  export type ReqBody = Pick<HistoryEntry, "chapterId">;
  export type ResBody = HistoryEntry;
}
