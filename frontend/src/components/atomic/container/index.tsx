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
  const classNames = [styles.container, className ?? ""];

  if (main) {
    return <main className={classNames.join(" ")}>{children}</main>;
  }

  return <div className={classNames.join(" ")}>{children}</div>;
}
