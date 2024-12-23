import { cx } from "@/util/classNames";
import styles from "./label.module.css";

export interface LabelProps {
  icon: React.ReactNode;
  label: string;
  className?: string;
  title?: string;
}

export default function Label({ icon, label, className, title }: LabelProps) {
  return (
    <span className={cx(styles.label, className)} title={title}>
      {icon}
      <p>{label}</p>
    </span>
  );
}
