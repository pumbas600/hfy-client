import { HeadMeta } from "@/components/atomic";
import { Header, Main, Sticky } from "@/components/layout/pageLayout";
import PrimaryLayout from "@/components/layout/primaryLayout";
import config from "@/config";
import styles from "@/components/home/home.module.css";

export default function Home() {
  return (
    <>
      <HeadMeta title={config.title} description={config.description} />
      <PrimaryLayout>
        <Sticky />
        <Header className={styles.homeHeader}>
          <h2>{config.title}</h2>
          <p>Optimizing your Reddit reading experience.</p>
        </Header>
        <Main noInlinePadding>
          <h1>Home</h1>
        </Main>
      </PrimaryLayout>
    </>
  );
}
