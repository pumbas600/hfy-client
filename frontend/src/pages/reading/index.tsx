import { HeadMeta } from "@/components/atomic";
import { Aside, Header, PageLayout } from "@/components/layout/pageLayout";
import config from "@/config";

export default function ReadingPage() {
  return (
    <>
      <HeadMeta title={`Reading | ${config.title}`} />
      <PageLayout>
        <Header>
          <h2>Reading</h2>
          <p>Recently read stories</p>
        </Header>
        <Aside />
      </PageLayout>
    </>
  );
}
