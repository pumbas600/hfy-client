import { Main, PageLayout } from "@/components/layout/pageLayout";
import LoginCard from "@/components/loginAndAuthorize/loginCard";
import LoginLayout from "@/components/loginAndAuthorize/loginLayout";
import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { PostLoginRequest } from "@/types/api";
import { Api } from "@/util/api";
import { useRouter } from "next/router";
import { useEffect, useRef } from "react";

function stateMatches(state?: string | string[]): boolean {
  return (
    typeof state === "string" &&
    decodeURIComponent(state) ===
      localStorage.getItem(LocalStorageKeys.redditState)
  );
}

export default function AuthorizePage() {
  const router = useRouter();
  const lastCode = useRef<string>();
  const { error, code, state } = router.query;

  const isStateCorrect = stateMatches(state);

  useEffect(() => {
    const login = async (code: string): Promise<void> => {
      if (code === lastCode.current || typeof code !== "string") {
        return;
      }

      lastCode.current = code;

      try {
        var userDto = await Api.post<PostLoginRequest.ResBody>(
          `${config.api.baseUrl}/users/login`,
          { redditCode: code },
          { refreshOnUnauthorized: false }
        );
        console.log("[Authorize]: Logged in");
        console.log(userDto);

        router.push("/");
      } catch (error) {
        console.error(error);
      }
    };

    if (typeof code === "string" && isStateCorrect) {
      login(code);
    }
  }, [router]);

  return (
    <LoginLayout>
      <Main>
        <LoginCard
          title="Authorizing"
          primaryLinkUrl="/"
          primaryLinkChildren="Go home"
        />
        {error && <p>Failed to log in: {error}</p>}
        {!isStateCorrect && (
          <p>There's something suspicious about this login request...</p>
        )}
      </Main>
    </LoginLayout>
  );
}
