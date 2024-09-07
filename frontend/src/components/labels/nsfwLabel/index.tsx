import NsfwIcon from "@/icons/nsfwIcon";
import styles from "./nsfwLabel.module.css";

export default function NsfwLabel() {
  return (
    <span className={styles.label}>
      <NsfwIcon />
      <p>NSFW</p>
    </span>
  );
}
