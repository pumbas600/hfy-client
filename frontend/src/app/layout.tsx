import type { Metadata } from "next";
import "@/styles/styles.css";

// See: https://docs.fontawesome.com/web/use-with/react/use-with#getting-font-awesome-css-to-work
import { config as fontAwesomeConfig } from "@fortawesome/fontawesome-svg-core";
import "@fortawesome/fontawesome-svg-core/styles.css";
import config from "@/config";
fontAwesomeConfig.autoAddCss = false;

export const metadata: Metadata = {
  title: config.title,
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
