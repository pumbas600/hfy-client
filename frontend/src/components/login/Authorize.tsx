"use client";

import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { Api } from "@/util/api";
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
    const login = async (): Promise<void> => {
      try {
        await Api.post(`${config.api.baseUrl}/users/login`, {
          redditCode: code,
        });
        console.log("[Authorize] Logged in");
      } catch (error) {
        console.error(error);
      }
    };

    if (state && state === getState(state) && code) {
      login();
    }
  }, [code]);

  if (state && state !== getState(state)) {
    return <div>There's something suspicious about this login request...</div>;
  }

  return null;
}
