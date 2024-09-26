import AuthorizationHandler from "@/components/authorize/AuthorizationHandler";
import { Main, PageLayout } from "@/components/layout/pageLayout";
import { Params } from "@/types/next";

export default function Authorize({
  searchParams,
}: Params<never, { error?: string; code?: string; state?: string }>) {
  if (searchParams.error) {
    return <div>Failed to log in: {searchParams.error}</div>;
  }

  return (
    <PageLayout>
      <AuthorizationHandler />
      <Main>
        <h2>Authorizing...</h2>
      </Main>
    </PageLayout>
  );
}
