"use client";

import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { Api } from "@/util/api";
import { useEffect } from "react";

const IS_SERVER = typeof window === "undefined";

export interface AuthorizeProps {
  code?: string;
  state?: string;
}

function stateMatches(state?: string): boolean {
  if (IS_SERVER || state === undefined) {
    return false;
  }

  return (
    decodeURIComponent(state) ===
    localStorage.getItem(LocalStorageKeys.redditState)
  );
}

export default function AuthorizationHandler({ code, state }: AuthorizeProps) {
  useEffect(() => {
    const login = async (): Promise<void> => {
      try {
        await Api.post(`${config.api.baseUrl}/users/login`, {
          redditCode: code,
        });
        console.log("[Authorize]: Logged in");
      } catch (error) {
        console.error(error);
      }
    };

    if (state && stateMatches(state) && code) {
      login();
    }
  }, [code]);

  if (state && !stateMatches(state)) {
    return <div>There's something suspicious about this login request...</div>;
  }

  return null;
}
