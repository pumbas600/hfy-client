import React from "react";
import styles from "./typography.module.css";

export interface SubtitleProps {
  className?: string;
  children?: React.ReactNode;
}

export function Subtitle({ children, className }: SubtitleProps) {
  return (
    <h6 className={`${styles.subtitle} ${className ?? ""}`}>{children}</h6>
  );
}
