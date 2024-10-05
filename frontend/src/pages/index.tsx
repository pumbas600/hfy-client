import { Card, HeadMeta, Link } from "@/components/atomic";
import { Header, Main, Sticky } from "@/components/layout/pageLayout";
import PrimaryLayout from "@/components/layout/primaryLayout";
import config from "@/config";
import styles from "@/components/home/home.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons/faArrowRight";

export default function Home() {
  return (
    <>
      <HeadMeta title={config.title} description={config.description} />
      <PrimaryLayout>
        <Sticky />
        <Header className={styles.homeHeader}>
          <h2>{config.title}</h2>
          <p>Optimizing your Reddit reading experience.</p>
        </Header>
        <Main noInlinePadding>
          <Card>
            <h3>{config.title} is in beta</h3>

            <Link href="/login" variant="largeButton">
              Login <FontAwesomeIcon size="lg" icon={faArrowRight} />
            </Link>
          </Card>
        </Main>
      </PrimaryLayout>
    </>
  );
}
