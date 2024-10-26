import { Link, Card } from "@/components/atomic";
import styles from "./loginCard.module.css";
import config from "@/config";
import { ReactNode } from "react";
import HomeIcon from "@/components/composite/homeIcon";

interface LoginCardProps {
  title: string;
  children: ReactNode;
  isLinkVisible?: boolean;
  primaryLinkUrl: string;
  primaryLinkChildren: ReactNode;
}

export default function LoginCard({
  title,
  children,
  isLinkVisible = true,
  primaryLinkUrl,
  primaryLinkChildren,
}: LoginCardProps) {
  return (
    <Card className={styles.loginCard}>
      <HomeIcon hideTitle className={styles.icon} />
      <div>
        <h3>{title}</h3>
        <h2>{config.title}</h2>
      </div>
      <p className={styles.content}>{children}</p>
      {isLinkVisible && (
        <Link variant="largeButton" href={primaryLinkUrl} newTab={false}>
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
