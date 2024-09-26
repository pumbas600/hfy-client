import BackButton from "@/components/composite/backButton";
import { Main, PageLayout } from "@/components/layout/pageLayout";
import RedditLogin from "@/components/login/RedditLogin";
import config from "@/config";
import { GetAuthorizationUrlRequest } from "@/types/api";
import { AuthorizationUrlDto } from "@/types/user";
import { Api } from "@/util/api";

export default async function Login() {
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
        <RedditLogin authorizationUrl={authorizationUrl} />
      </Main>
    </PageLayout>
  );
}
