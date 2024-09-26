import AuthorizationHandler from "@/components/authorize/AuthorizationHandler";
import { Main, PageLayout } from "@/components/layout/pageLayout";
import { Params } from "@/types/next";

export default function Authorize({
  searchParams,
}: Params<never, { error?: string; code?: string; state?: string }>) {
  return (
    <PageLayout>
      {searchParams.error ? (
        <div>Failed to log in: {searchParams.error}</div>
      ) : (
        <AuthorizationHandler
          code={searchParams.code}
          state={searchParams.state}
        />
      )}
      <Main>
        <h2>Authorizing...</h2>
      </Main>
    </PageLayout>
  );
}
