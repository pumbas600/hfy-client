import { Link } from "@/components/atomic";
import BackButton from "@/components/composite/backButton";
import { Main, PageLayout, Sticky } from "@/components/layout/pageLayout";
import LoginCard from "@/components/loginAndAuthorize/loginCard";
import LoginLayout from "@/components/loginAndAuthorize/loginLayout";
import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { GetAuthorizationUrlRequest } from "@/types/api";
import { Api } from "@/util/api";
import { randomBytes } from "crypto";
import { GetServerSideProps } from "next";
import { useEffect } from "react";

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
    <LoginLayout>
      <Sticky start={<BackButton />} />
      <Main noInlinePadding>
        <LoginCard redditAuthUrl={authorizationUrl} />
      </Main>
    </LoginLayout>
  );
}
