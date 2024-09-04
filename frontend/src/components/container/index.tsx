import styles from "./container.module.css";
import { ReactNode } from "react";

export interface ContainerProps {
  children?: ReactNode;
  className?: string;
  main?: boolean;
}

export default function Container({
  children,
  className,
  main = false,
}: ContainerProps) {
  if (main) {
    return (
      <main className={`${styles.container} ${className}`}>{children}</main>
    );
  }

  return <div className={`${styles.container} ${className}`}>{children}</div>;
}
