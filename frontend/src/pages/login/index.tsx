import BackButton from "@/components/composite/backButton";
import { Main, PageLayout } from "@/components/layout/pageLayout";
import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { GetAuthorizationUrlRequest } from "@/types/api";
import { Api } from "@/util/api";
import { randomBytes } from "crypto";
import { GetServerSideProps } from "next";
import Link from "next/link";
import { useEffect } from "react";

function generateRandomString(length: number): string {
  return randomBytes(length).toString("hex");
}

interface LoginPageProps {
  authorizationUrl: string;
  state: string;
}

export const getServerSideProps = (async () => {
  const { url } = await Api.get<GetAuthorizationUrlRequest.ResBody>(
    `${config.api.baseUrl}/users/reddit/authorize`
  );

  const state = generateRandomString(32);
  const authorizationUrl = new URL(url);
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
    <PageLayout>
      <Main>
        <BackButton />
        <Link href={authorizationUrl}>
          <button>Login with Reddit</button>
        </Link>
      </Main>
    </PageLayout>
  );
}
