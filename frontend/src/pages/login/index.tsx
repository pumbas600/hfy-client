import BackButton from "@/components/composite/backButton";
import { Main, Sticky } from "@/components/layout/pageLayout";
import LoginCard from "@/components/loginAndAuthorize/loginCard";
import PrimaryLayout from "@/components/layout/primaryLayout";
import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { GetAuthorizationUrlRequest } from "@/types/api";
import { Api } from "@/util/api";
import { faReddit } from "@fortawesome/free-brands-svg-icons/faReddit";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { randomBytes } from "crypto";
import { GetServerSideProps } from "next";
import { useEffect } from "react";
import { HeadMeta } from "@/components/atomic";

function generateRandomString(length: number): string {
  return randomBytes(length).toString("hex");
}

interface LoginPageProps {
  authorizationUrl: string;
  state: string;
}

export const getServerSideProps = (async ({ req, res }) => {
  const urlResponse = await Api.get<GetAuthorizationUrlRequest.ResBody>(
    `${config.api.baseUrl}/users/reddit/authorize`,
    { req, res }
  );

  const state = generateRandomString(32);
  const authorizationUrl = new URL(urlResponse.data.url);
  authorizationUrl.searchParams.set("state", state);

  return {
    props: {
      authorizationUrl: authorizationUrl.toString(),
      state,
    },
  };
}) satisfies GetServerSideProps<LoginPageProps>;

export default function LoginPage({ authorizationUrl, state }: LoginPageProps) {
  useEffect(() => {
    console.log("[Authorize]: Setting reddit state in local storage");
    localStorage.setItem(LocalStorageKeys.redditState, state);
  }, [state]);

  return (
    <>
      <HeadMeta title={`Login | ${config.title}`} />
      <PrimaryLayout>
        <Sticky start={<BackButton />} />
        <Main noInlinePadding>
          <LoginCard
            title="Login"
            isLinkVisible
            primaryLinkUrl={authorizationUrl}
            primaryLinkChildren={
              <>
                <FontAwesomeIcon size="xl" icon={faReddit} /> Sign in with
                Reddit{" "}
              </>
            }
          >
            You are required to login with Reddit to access this website. This
            is used to verify that you have a Reddit account and to access your
            username and profile picture.
          </LoginCard>
        </Main>
      </PrimaryLayout>
    </>
  );
}
