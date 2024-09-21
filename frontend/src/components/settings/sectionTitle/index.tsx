import { HTMLAttributes } from "react";
import styles from "./sectionTitle.module.css";

type HeadingProps = HTMLAttributes<HTMLHeadingElement>;

export default function SectionTitle(props: HeadingProps) {
  return (
    <h3
      {...props}
      className={`${styles.sectionTitle} ${props.className ?? ""}`}
    />
  );
}
