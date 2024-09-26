"use client";

import { LocalStorageKeys } from "@/config/localStorage";
import { useEffect } from "react";

const IS_SERVER = typeof window === "undefined";

function getState(state: string): string | null {
  if (IS_SERVER || state === undefined) {
    return null;
  }

  return localStorage.getItem(LocalStorageKeys.redditState) ?? null;
}

export interface AuthorizeProps {
  code?: string;
  state?: string;
}

export default function Authorize({ code, state }: AuthorizeProps) {
  useEffect(() => {
    if (state && state === getState(state) && code) {
      // await Api.post()
    }
  }, [code]);

  if (state && state !== getState(state)) {
    return <div>There's something suspicious about this login request...</div>;
  }

  return null;
}
