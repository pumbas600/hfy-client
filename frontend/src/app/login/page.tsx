import BackButton from "@/components/composite/backButton";
import { Main, PageLayout } from "@/components/layout/pageLayout";
import Authorize from "@/components/login/Authorize";
import RedditLogin from "@/components/login/RedditLogin";
import config from "@/config";
import { LocalStorageKeys } from "@/config/localStorage";
import { GetAuthorizationUrlRequest } from "@/types/api";
import { Params } from "@/types/next";
import { AuthorizationUrlDto } from "@/types/user";
import { Api } from "@/util/api";

export default async function Login({
  searchParams,
}: Params<never, { error?: string; code?: string; state?: string }>) {
  let authorizationUrl: AuthorizationUrlDto;
  try {
    authorizationUrl = await Api.get<GetAuthorizationUrlRequest.ResBody>(
      `${config.api.baseUrl}/users/reddit/authorize`
    );
  } catch (error) {
    console.error(error);
    return <div>Failed to get authorization URL</div>;
  }

  if (searchParams.error) {
    return <div>Failed to log in: {searchParams.error}</div>;
  }

  return (
    <PageLayout>
      <Authorize code={searchParams.code} state={searchParams.state} />
      <Main>
        <BackButton />
        <RedditLogin authorizationUrl={authorizationUrl} />
      </Main>
    </PageLayout>
  );
}
