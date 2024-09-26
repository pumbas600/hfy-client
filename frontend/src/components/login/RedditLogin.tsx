"use client";

import { LocalStorageKeys } from "@/config/localStorage";
import Link from "next/link";
import { useEffect } from "react";

const IS_SERVER = typeof window === "undefined";

export interface RedditLoginProps {
  authorizationUrl: string;
  state: string;
}

export default function RedditLogin({
  authorizationUrl,
  state,
}: RedditLoginProps) {
  const url = new URL(authorizationUrl);
  url.searchParams.set("state", state);

  useEffect(() => {
    if (!IS_SERVER) {
      console.log("[Authorize]: Setting reddit state in local storage");
      localStorage.setItem(LocalStorageKeys.redditState, state);
    }
  }, [authorizationUrl]);

  return (
    <Link href={url}>
      <button>Login with Reddit</button>
    </Link>
  );
}
