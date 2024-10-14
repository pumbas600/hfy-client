import dynamic from "next/dynamic";
import { ReactNode } from "react";

interface NoSsrProps {
  children: ReactNode;
}

function NoSsrChildrenWrapper({ children }: NoSsrProps) {
  return <>{children}</>;
}

// Prevent SSR to avoid hydration issues.
const NoSsr = dynamic(() => Promise.resolve(NoSsrChildrenWrapper), {
  ssr: false,
});

export default NoSsr;
