import styles from "./container.module.css";
import { ReactNode } from "react";

export interface ContainerProps {
  children?: ReactNode;
  className?: string;
}

export default function Container({ children, className }: ContainerProps) {
  return <div className={`${styles.container} ${className}`}>{children}</div>;
}
