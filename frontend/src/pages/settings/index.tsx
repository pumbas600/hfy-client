import { HeadMeta, ProfilePicture } from "@/components/atomic";
import BackButton from "@/components/composite/backButton";
import SelfProfile from "@/components/composite/selfProfile";
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
import { GetInfo, GetSelf } from "@/types/api";
import { Info } from "@/types/info";
import { User } from "@/types/user";
import { Api } from "@/util/api";
import { GetServerSideProps } from "next";
import styles from "@/components/settings/settings.module.css";

interface SettingsPageProps {
  apiInfo: ApiInfoProps;
  self: User;
}

const ONE_DAY = 60 * 60 * 24;
const THIRTY_MINUTES = 30 * 60;

export const getServerSideProps = (async ({ req, res }) => {
  const infoUrl = `${config.api.baseUrl}/info`;
  const defaultInfo: Info = {
    apiVersion: "unknown",
    environment: "unknown",
  };

  const [infoResponse, selfResponse] = await Promise.all([
    Api.get<GetInfo.ResBody>(infoUrl, {
      default: defaultInfo,
      revalidate: ONE_DAY,
    }),
    Api.get<GetSelf.ResBody>(`${config.api.baseUrl}/users/@me`, {
      req,
      res,
      revalidate: THIRTY_MINUTES,
    }),
  ]);

  return {
    props: {
      apiInfo: { infoUrl, info: infoResponse.data },
      self: selfResponse.data,
    },
  };
}) satisfies GetServerSideProps<SettingsPageProps>;

export default function SettingsPage({ apiInfo, self }: SettingsPageProps) {
  return (
    <>
      <HeadMeta title={`Settings | ${config.title}`} />
      <PageLayout>
        <Sticky start={<BackButton />} end={<SelfProfile user={self} />} />
        <Header className={styles.header}>
          <ProfilePicture user={self} className={styles.profilePicture} />
          <div>
            <h2>Settings</h2>
            <h3>u/{self.name}</h3>
          </div>
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
