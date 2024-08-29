import Container from "@/components/container";
import { ReactChildren } from "@/types/next";

export default function Layout({ children }: ReactChildren) {
  return <Container>{children}</Container>;
}
