import BackButton from "@/components/composite/backButton";
import { Main, PageLayout } from "@/components/layout/pageLayout";
import RedditLogin from "@/components/login/RedditLogin";
import config from "@/config";
import { GetAuthorizationUrlRequest } from "@/types/api";
import { AuthorizationUrlDto } from "@/types/user";
import { Api } from "@/util/api";
import { randomBytes } from "crypto";

function generateRandomString(length: number): string {
  return randomBytes(length).toString("hex");
}

export default async function LoginPage() {
  const state = generateRandomString(32);
  let authorizationUrl: AuthorizationUrlDto;
  try {
    authorizationUrl = await Api.get<GetAuthorizationUrlRequest.ResBody>(
      `${config.api.baseUrl}/users/reddit/authorize`
    );
  } catch (error) {
    console.error(error);
    return <div>Failed to get authorization URL</div>;
  }

  return (
    <PageLayout>
      <Main>
        <BackButton />
        <RedditLogin authorizationUrl={authorizationUrl.url} state={state} />
      </Main>
    </PageLayout>
  );
}
