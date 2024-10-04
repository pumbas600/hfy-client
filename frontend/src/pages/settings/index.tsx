import { Button, HeadMeta, ProfilePicture } from "@/components/atomic";
import BackButton from "@/components/composite/backButton";
import {
  Aside,
  Header,
  Main,
  PageLayout,
  Sticky,
} from "@/components/layout/pageLayout";
import ApiInfo, { ApiInfoProps } from "@/components/settings/apiInfo";
import SectionTitle from "@/components/settings/sectionTitle";
import ThemeSelect from "@/components/settings/themeSelect";
import config from "@/config";
import { GetInfo } from "@/types/api";
import { Info } from "@/types/info";
import { Api } from "@/util/api";
import { GetStaticProps } from "next";
import styles from "@/components/settings/settings.module.css";
import LogoutButton from "@/components/composite/LogoutButton";
import { getSelf } from "@/lib/getSelf";

interface SettingsPageProps {
  apiInfo: ApiInfoProps;
}

const ONE_DAY = 60 * 60 * 24;

export const getStaticProps = (async () => {
  const infoUrl = `${config.api.baseUrl}/info`;
  const defaultInfo: Info = {
    apiVersion: "unknown",
    environment: "unknown",
  };

  const infoResponse = await Api.get<GetInfo.ResBody>(infoUrl, {
    default: defaultInfo,
  });

  return {
    props: {
      apiInfo: { infoUrl, info: infoResponse.data },
    },
    revalidate: ONE_DAY,
  };
}) satisfies GetStaticProps<SettingsPageProps>;

export default function SettingsPage({ apiInfo }: SettingsPageProps) {
  const self = getSelf();

  return (
    <>
      <HeadMeta title={`Settings | ${config.title}`} />
      <PageLayout>
        <Sticky start={<BackButton />} />
        <Header className={styles.header}>
          {self && (
            <ProfilePicture user={self} className={styles.profilePicture} />
          )}
          <div>
            <h2>Settings</h2>
            <h3>u/{self?.name}</h3>
          </div>
          <LogoutButton />
        </Header>
        <Aside />
        <Main>
          <SectionTitle>Appearance</SectionTitle>
          <ThemeSelect />
          <SectionTitle>Behind the scenes</SectionTitle>
          <ApiInfo {...apiInfo} />
        </Main>
      </PageLayout>
    </>
  );
}
