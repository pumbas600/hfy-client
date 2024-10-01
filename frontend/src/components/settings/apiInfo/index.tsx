import { SecureBadge } from "@/components/composite/badges";
import styles from "./apiInfo.module.css";
import { Info } from "@/types/info";

export interface ApiInfoProps {
  infoUrl: string;
  info: Info;
}

export default function ApiInfo({ infoUrl, info }: ApiInfoProps) {
  const infoUrlDetails = new URL(infoUrl);
  const isApiSecure = infoUrlDetails.protocol === "https:";

  return (
    <div className={styles.apiInfo}>
      <p className={styles.title}>API URL</p>
      <div className={styles.urlWrapper}>
        <p>{infoUrlDetails.hostname}</p>
        <SecureBadge isSecure={isApiSecure} />
      </div>
      <p className={styles.title}>Version</p>
      <p>{info.apiVersion}</p>
      <p className={styles.title}>Environment</p>
      <p>{info.environment}</p>
    </div>
  );
}
