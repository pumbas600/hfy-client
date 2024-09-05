import React from "react";
import styles from "./textLayout.module.css";

export interface TextLayoutProps {
  textHtml?: string;
  children?: React.ReactNode;
}

export default function TextLayout({ textHtml, children }: TextLayoutProps) {
  return (
    <main
      className={styles.container}
      dangerouslySetInnerHTML={textHtml ? { __html: textHtml } : undefined}
    >
      {children}
    </main>
  );
}
