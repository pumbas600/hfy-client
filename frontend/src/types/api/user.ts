import { AuthorizationUrlDto, User } from "../user";

export namespace GetSelf {
  export type Params = never;

  export type ReqBody = never;
  export type ResBody = User;
}

export namespace GetAuthorizationUrlRequest {
  export type Params = never;

  export type ReqBody = never;
  export type ResBody = AuthorizationUrlDto;
}

export namespace PostLoginRequest {
  export type Params = never;

  export type ReqBody = {
    redditCode: string;
  };
  export type ResBody = User;
}
