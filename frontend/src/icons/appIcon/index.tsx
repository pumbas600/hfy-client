import styles from "./appIcon.module.css";

export interface AppIconProps {
  inverted?: boolean;
  className?: string;
}

export default function AppIcon({ inverted, className }: AppIconProps) {
  const classes = [
    styles.appIcon,
    inverted ? styles.invertedAppIcon : styles.normalAppIcon,
  ];
  if (className) {
    classes.push(className);
  }

  return (
    <svg
      width="32"
      height="32"
      viewBox="0 0 32 32"
      xmlns="http://www.w3.org/2000/svg"
      className={classes.join(" ")}
    >
      <rect width="32" height="32" rx="4" fill="var(--icon-background-color)" />
      <path
        d="M26 16C26 17.9778 25.4135 19.9112 24.3147 21.5557C23.2159 23.2002 21.6541 24.4819 19.8268 25.2388C17.9996 25.9957 15.9889 26.1937 14.0491 25.8079C12.1093 25.422 10.3275 24.4696 8.92893 23.0711C7.53041 21.6725 6.578 19.8907 6.19215 17.9509C5.80629 16.0111 6.00433 14.0004 6.7612 12.1732C7.51808 10.3459 8.79981 8.78412 10.4443 7.6853C12.0888 6.58649 14.0222 6 16 6V16H26Z"
        fill="var(--icon-foreground-color)"
      />
      <rect
        x="26"
        y="14"
        width="8"
        height="8"
        rx="4"
        transform="rotate(-180 26 14)"
        fill="var(--icon-foreground-color)"
      />
    </svg>
  );
}
