import type { Metadata } from "next";
import "@/styles/styles.css";

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
