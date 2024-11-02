import { ReactNode } from "react";
import styles from "./card.module.css";
import { cx } from "@/util/classNames";

interface CardProps {
  children?: ReactNode;
  className?: string;
}

export default function Card({ children, className }: CardProps) {
  return <div className={cx(styles.card, className)}>{children}</div>;
}
