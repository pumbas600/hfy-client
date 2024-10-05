import { Link, Card } from "@/components/atomic";
import styles from "./loginCard.module.css";
import config from "@/config";
import { ReactNode } from "react";

interface LoginCardProps {
  title: string;
  children: ReactNode;
  isLinkVisible: boolean;
  primaryLinkUrl: string;
  primaryLinkChildren: ReactNode;
}

export default function LoginCard({
  title,
  children,
  isLinkVisible,
  primaryLinkUrl,
  primaryLinkChildren,
}: LoginCardProps) {
  return (
    <Card className={styles.loginCard}>
      <Link href="/" className={styles.iconLink}>
        <img
          src="https://styles.redditmedia.com/t5_2y95n/styles/communityIcon_or09gmyen4l21.png"
          alt="HFY Logo"
          className={styles.icon}
        />
      </Link>
      <div>
        <h3>{title}</h3>
        <h2>{config.title}</h2>
      </div>
      <p className={styles.content}>{children}</p>
      {isLinkVisible && (
        <Link variant="largeButton" href={primaryLinkUrl}>
          {primaryLinkChildren}
        </Link>
      )}
      <p>
        Learn more about{" "}
        <Link href="/about" variant="subtle" className={styles.inlineLink}>
          {config.title}
        </Link>
        .
      </p>
    </Card>
  );
}
