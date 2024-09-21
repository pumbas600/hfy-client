import { SecureBadge } from "@/components/composite/badges";
import config from "@/config";
import { GetInfo } from "@/types/api";
import { Api } from "@/util/api";
import styles from "./apiInfo.module.css";

export default async function ApiInfo() {
  const infoUrl = new URL(`${config.api.baseUrl}/info`);
  const isApiSecure = infoUrl.protocol === "https:";

  const apiInfo = await Api.get<GetInfo.ResBody>(infoUrl, {
    default: {
      apiVersion: "unknown",
      environment: "unknown",
    },
  });

  return (
    <>
      <h4>Application Details</h4>
      <div className={styles.apiInfo}>
        <p className={styles.title}>URL</p>
        <div className={styles.urlWrapper}>
          <p>{infoUrl.hostname}</p>
          <SecureBadge isSecure={isApiSecure} />
        </div>
        <p className={styles.title}>Version</p>
        <p>{apiInfo.apiVersion}</p>
        <p className={styles.title}>Environment</p>
        <p>{apiInfo.environment}</p>
      </div>
    </>
  );
}
