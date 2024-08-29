import { ReactChildren } from "@/types/next";
import styles from "./container.module.css";

export default function Container({ children }: ReactChildren) {
  return <div className={styles.container}>{children}</div>;
}
