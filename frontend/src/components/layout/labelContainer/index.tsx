import { ReactChildren } from "@/types/next";
import styles from "./labelContainer.module.css";

export default function LabelContainer({ children }: ReactChildren) {
  return <div className={styles.labelContainer}>{children}</div>;
}
