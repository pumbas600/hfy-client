import {
  Aside,
  Header,
  Main,
  PageLayout,
} from "@/components/layout/pageLayout";
import ApiInfo from "@/components/settings/apiInfo";

export default async function Settings() {
  return (
    <PageLayout>
      <Header>
        <h1>Settings</h1>
        <h3>u/pumbas600</h3>
      </Header>
      <Aside />
      <Main>
        <ApiInfo />

        <form>
          <select>
            <option>Dark</option>
          </select>
        </form>
      </Main>
    </PageLayout>
  );
}
