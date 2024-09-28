import { HeadMeta } from "@/components/atomic";
import {
  Aside,
  Header,
  Main,
  PageLayout,
} from "@/components/layout/pageLayout";
import ApiInfo, { ApiInfoProps } from "@/components/settings/apiInfo";
import SectionTitle from "@/components/settings/sectionTitle";
import ThemeSelect from "@/components/settings/themeSelect";
import config from "@/config";
import { GetInfo } from "@/types/api";
import { Api } from "@/util/api";
import { GetStaticProps } from "next";

const ONE_DAY = 60 * 60 * 24;

export const getStaticProps = (async () => {
  const infoUrl = `${config.api.baseUrl}/info`;

  const infoResponse = await Api.get<GetInfo.ResBody>(infoUrl, {
    default: {
      apiVersion: "unknown",
      environment: "unknown",
    },
  });

  return {
    props: { infoUrl, info: infoResponse.data },
    revalidate: ONE_DAY,
  };
}) satisfies GetStaticProps<ApiInfoProps>;

export default function Settings(infoProps: ApiInfoProps) {
  return (
    <>
      <HeadMeta title={`Settings | ${config.title}`} />
      <PageLayout>
        <Header>
          <h1>Settings</h1>
          <h3>u/pumbas600</h3>
        </Header>
        <Aside />
        <Main>
          <SectionTitle>Appearance</SectionTitle>
          <ThemeSelect />

          <SectionTitle>Behind the scenes</SectionTitle>
          <ApiInfo {...infoProps} />
        </Main>
      </PageLayout>
    </>
  );
}
