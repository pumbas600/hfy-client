import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./styles/styles.css";

const inter = Inter({ subsets: ["latin"] });

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
      <body className={inter.className}>{children}</body>
    </html>
  );
}
