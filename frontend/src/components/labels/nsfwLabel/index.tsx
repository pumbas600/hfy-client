import NsfwIcon from "@/icons/nsfwIcon";
import styles from "./nsfwLabel.module.css";
import Label from "../label";

export default function NsfwLabel() {
  return <Label icon={<NsfwIcon />} label="NSFW" className={styles.label} />;
}
