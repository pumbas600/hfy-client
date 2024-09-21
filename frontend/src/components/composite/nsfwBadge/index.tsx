import NsfwIcon from "@/icons/nsfwIcon";
import styles from "./nsfwLabel.module.css";
import { Badge } from "@/components/atomic";

export default function NsfwBadge() {
  return <Badge icon={<NsfwIcon />} label="NSFW" className={styles.nsfw} />;
}
