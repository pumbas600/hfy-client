import Label, { LabelProps } from "../label";
import styles from "./badge.module.css";

export default function Badge(props: LabelProps) {
  return (
    <Label {...props} className={`${styles.badge} ${props.className ?? ""}`} />
  );
}
