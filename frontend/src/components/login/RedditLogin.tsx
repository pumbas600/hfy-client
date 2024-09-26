"use client";

import { LocalStorageKeys } from "@/config/localStorage";
import Link from "next/link";
import { useEffect } from "react";
import { randomBytes } from "crypto";

const IS_SERVER = typeof window === "undefined";

export interface RedditLoginProps {
  authorizationUrl: string;
}

function generateRandomString(length: number): string {
  return randomBytes(length).toString("hex");
}

export default function RedditLogin({ authorizationUrl }: RedditLoginProps) {
  const state = generateRandomString(32);
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
