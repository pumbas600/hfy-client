import AuthorizationHandler from "@/components/authorize/AuthorizationHandler";
import { Params } from "@/types/next";

export default function Authorize({
  searchParams,
}: Params<never, { error?: string; code?: string; state?: string }>) {
  if (searchParams.error) {
    return <div>Failed to log in: {searchParams.error}</div>;
  }

  return <AuthorizationHandler />;
}
