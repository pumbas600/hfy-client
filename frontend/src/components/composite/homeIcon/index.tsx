import AppIcon, { AppIconProps } from "@/icons/appIcon";
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
  const appIconClasses = [styles.appIcon];

  if (props.inverted) {
    appIconClasses.push(styles.inverted);
  }

  if (props.className) {
    appIconClasses.push(props.className);
  }

  return (
    <Link href="/" className={styles.homeIcon} aria-label="Go to home page">
      <AppIcon {...props} className={appIconClasses.join(" ")} />
      {!hideTitle && <p>{config.title}</p>}
    </Link>
  );
}
