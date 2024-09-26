import { Subreddit } from "../subreddit";

export namespace GetSubredditRequest {
  export interface Params {
    name: Subreddit["name"];
  }

  export type ReqBody = never;
  export type ResBody = Subreddit;
}
