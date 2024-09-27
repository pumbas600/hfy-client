"use client";

import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { PostLoginRequest } from "@/types/api";
import { Api } from "@/util/api";
import { useRouter } from "next/navigation";
import { useEffect, useRef } from "react";

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
  const lastCode = useRef<string>();
  const router = useRouter();

  useEffect(() => {
    const login = async (): Promise<void> => {
      if (code === lastCode.current) {
        return;
      }

      lastCode.current = code;

      try {
        var userDto = await Api.post<PostLoginRequest.ResBody>(
          `${config.api.baseUrl}/users/login`,
          { redditCode: code }
        );
        console.log("[Authorize]: Logged in");
        console.log(userDto);

        router.push("/");
      } catch (error) {
        console.error(error);
      }
    };

    if (!IS_SERVER && state && stateMatches(state) && code) {
      login();
    }
  }, [code, state, router]);

  if (state && !stateMatches(state)) {
    return <div>There's something suspicious about this login request...</div>;
  }

  return null;
}
