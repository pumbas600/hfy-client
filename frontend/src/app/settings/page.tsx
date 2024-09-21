import { SecureBadge } from "@/components/composite/badges";
import {
  Aside,
  Header,
  Main,
  PageLayout,
} from "@/components/layout/pageLayout";
import config from "@/config";
import { GetInfo } from "@/types/api";
import { Api } from "@/util/api";

export default async function Settings() {
  const infoUrl = new URL(`${config.api.baseUrl}/info`);
  const isApiSecure = infoUrl.protocol === "https:";

  const apiInfo = await Api.get<GetInfo.ResBody>(infoUrl, {
    default: {
      apiVersion: "unknown",
      environment: "unknown",
    },
  });

  return (
    <PageLayout>
      <Header>
        <h1>Settings</h1>
        <h3>u/pumbas600</h3>
      </Header>
      <Aside />
      <Main>
        <p>API</p>
        <div>
          <p>url: {infoUrl.hostname}</p> <SecureBadge isSecure={isApiSecure} />
        </div>
        <p>version: {apiInfo.apiVersion}</p>
        <p>environment: {apiInfo.environment}</p>

        <form>
          <select>
            <option>Dark</option>
          </select>
        </form>
      </Main>
    </PageLayout>
  );
}
