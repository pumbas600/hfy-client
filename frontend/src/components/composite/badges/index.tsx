import NsfwIcon from "@/icons/nsfwIcon";
import styles from "./badges.module.css";
import { Badge } from "@/components/atomic";

export function NsfwBadge() {
  return <Badge icon={<NsfwIcon />} label="NSFW" className={styles.nsfw} />;
}
