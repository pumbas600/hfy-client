import { HeadMeta } from "@/components/atomic";
import { Header, Main, Sticky } from "@/components/layout/pageLayout";
import PrimaryLayout from "@/components/layout/primaryLayout";
import config from "@/config";

export default function Home() {
  return (
    <>
      <HeadMeta title={config.title} description={config.description} />
      <PrimaryLayout>
        <Sticky />
        <Header>
          <h2>{config.title}</h2>
          <h5>Optimizing your Reddit reading experience.</h5>
        </Header>
        <Main noInlinePadding>
          <h1>Home</h1>
        </Main>
      </PrimaryLayout>
    </>
  );
}
