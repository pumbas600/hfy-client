import type { Metadata } from "next";
import dayjs from "dayjs";
import relativeTime from "dayjs/plugin/relativeTime";
import "@/styles/styles.css";

// See: https://docs.fontawesome.com/web/use-with/react/use-with#getting-font-awesome-css-to-work
import { config } from "@fortawesome/fontawesome-svg-core";
import "@fortawesome/fontawesome-svg-core/styles.css";
config.autoAddCss = false;

dayjs.extend(relativeTime);

export const metadata: Metadata = {
  title: "HFY Client",
  description: "A reddit client for browsing the r/HFY subreddit.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
