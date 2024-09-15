import React from "react";
import styles from "./typography.module.css";

export interface SubtitleProps {
  children?: React.ReactNode;
}

export function Subtitle({ children }: SubtitleProps) {
  return <h6 className={styles.subtitle}>{children}</h6>;
}
