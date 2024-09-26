import { AuthorizationUrlDto, UserDto } from "../user";

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
  export type ResBody = UserDto;
}
