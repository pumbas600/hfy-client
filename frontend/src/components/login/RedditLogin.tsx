"use client";

import { LocalStorageKeys } from "@/config/localStorage";
import { AuthorizationUrlDto } from "@/types/user";
import Link from "next/link";
import { useEffect } from "react";

const IS_SERVER = typeof window === "undefined";

export interface RedditLoginProps {
  authorizationUrl: AuthorizationUrlDto;
}

export default function RedditLogin({ authorizationUrl }: RedditLoginProps) {
  useEffect(() => {
    if (!IS_SERVER) {
      localStorage.setItem(
        LocalStorageKeys.redditState,
        authorizationUrl.state
      );
    }
  }, [authorizationUrl]);

  return (
    <Link href={authorizationUrl.url}>
      <button>Login with Reddit</button>
    </Link>
  );
}
