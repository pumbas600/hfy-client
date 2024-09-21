import NsfwIcon from "@/icons/nsfwIcon";
import styles from "./badges.module.css";
import { Badge } from "@/components/atomic";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLock } from "@fortawesome/free-solid-svg-icons/faLock";
import { faLockOpen } from "@fortawesome/free-solid-svg-icons/faLockOpen";

export function NsfwBadge() {
  return <Badge icon={<NsfwIcon />} label="NSFW" className={styles.nsfw} />;
}

export interface SecureBadgeProps {
  isSecure?: boolean;
}

export function SecureBadge({ isSecure }: SecureBadgeProps) {
  const icon = isSecure ? faLock : faLockOpen;
  const label = isSecure ? "Secure" : "Insecure";

  return (
    <Badge
      icon={<FontAwesomeIcon icon={icon} />}
      label={label}
      className={styles.secure}
    />
  );
}
