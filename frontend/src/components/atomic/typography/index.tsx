import React, { ReactNode } from "react";
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

interface TextProps {
  children?: ReactNode;
  color?: "primaryContrastSecondary";
}

export function Text({ children, color }: TextProps) {
  let colorClass = "";
  switch (color) {
    case "primaryContrastSecondary":
      colorClass = styles.primaryContrastSecondary;
      break;
  }

  return <p className={`${styles.text} ${colorClass ?? ""}`}>{children}</p>;
}
