import { AnchorHTMLAttributes, DetailedHTMLProps } from "react";
import styles from "./subtleLink.module.css";

export default function SubtleLink(
  props: Omit<
    DetailedHTMLProps<
      AnchorHTMLAttributes<HTMLAnchorElement>,
      HTMLAnchorElement
    >,
    "className"
  >
) {
  return <a {...props} className={styles.subtleLink} />;
}
