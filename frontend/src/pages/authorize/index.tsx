import { Main, PageLayout } from "@/components/layout/pageLayout";
import LoginCard from "@/components/loginAndAuthorize/loginCard";
import LoginLayout from "@/components/loginAndAuthorize/loginLayout";
import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { PostLoginRequest } from "@/types/api";
import { Api, ApiError } from "@/util/api";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons/faArrowRight";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useRouter } from "next/router";
import { useEffect, useRef, useState } from "react";

function stateMatches(state?: string | string[]): boolean {
  return (
    typeof state === "string" &&
    decodeURIComponent(state) ===
      localStorage.getItem(LocalStorageKeys.redditState)
  );
}

function determineContent(isStateCorrect: boolean, error?: string | string[]) {
  /* https://github.com/reddit-archive/reddit/wiki/OAuth2#token-retrieval-code-flow */
  if (error === "access_denied") {
    return (
      <>
        Unfortunately, to access this site you must login with your Reddit
        account. This is required to verify you have a Reddit account and to
        determine your username and profile picture.
      </>
    );
  }
  if (!isStateCorrect || error) {
    return "Hmm… Something went wrong while logging in with Reddit. Please try again.";
  }

  return "Verifying Reddit login attempt… Please wait a moment while we redirect you.";
}

export default function AuthorizePage() {
  const router = useRouter();
  const [isNotWhitelisted, setIsNotWhitelisted] = useState(false);
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
        if (error instanceof ApiError && error.code === "User.NotWhitelisted") {
          setIsNotWhitelisted(true);
        }
      }
    };

    if (typeof code === "string" && isStateCorrect) {
      login(code);
    }
  }, [router]);

  const content = determineContent(isStateCorrect, error);
  const isLinkVisible =
    !isStateCorrect || error !== undefined || isNotWhitelisted;

  const primaryLink = isNotWhitelisted
    ? { url: "/", label: "Learn more" }
    : { url: "/login", label: "Try again" };

  return (
    <LoginLayout>
      <Main noInlinePadding>
        <LoginCard
          title="Authorizing"
          isLinkVisible={isLinkVisible}
          primaryLinkUrl={primaryLink.url}
          primaryLinkChildren={
            <>
              {primaryLink.label}{" "}
              <FontAwesomeIcon size="xl" icon={faArrowRight} />
            </>
          }
        >
          {content}
        </LoginCard>
      </Main>
    </LoginLayout>
  );
}
