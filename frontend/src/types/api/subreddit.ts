import { Subreddit } from "../subreddit";

export namespace GetSubredditRequest {
  export interface Params {
    name: Subreddit["name"];
  }

  export type ResBody = Subreddit;
  export type ReqBody = never;
}
