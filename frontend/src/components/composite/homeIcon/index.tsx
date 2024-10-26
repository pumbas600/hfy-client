import AppIcon, { AppIconProps } from "@/icons/AppIcon";
import Link from "next/link";
import styles from "./homeIcon.module.css";
import config from "@/config";

interface HomeIconProps extends AppIconProps {
  hideTitle?: boolean;
}

export default function HomeIcon({
  hideTitle = false,
  ...props
}: HomeIconProps) {
  return (
    <Link href="/" className={styles.homeIcon}>
      <AppIcon {...props} />
      {!hideTitle && <p>{config.title}</p>}
    </Link>
  );
}
