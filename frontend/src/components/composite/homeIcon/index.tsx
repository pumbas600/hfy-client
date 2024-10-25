import AppIcon, { AppIconProps } from "@/icons/AppIcon";
import Link from "next/link";
import styles from "./homeIcon.module.css";
import config from "@/config";

export default function HomeIcon(props: Omit<AppIconProps, "className">) {
  return (
    <Link href="/" className={styles.homeIcon}>
      <AppIcon {...props} />
      <p>{config.title}</p>
    </Link>
  );
}
