import { Card, HeadMeta, Link } from "@/components/atomic";
import { Header, Main, Sticky } from "@/components/layout/pageLayout";
import PrimaryLayout from "@/components/layout/primaryLayout";
import config from "@/config";
import styles from "@/components/home/home.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons/faArrowRight";
import { WhitelistMessage } from "@/config/constants";
import LoginCard from "@/components/loginAndAuthorize/loginCard";

export default function Home() {
  return (
    <>
      <HeadMeta title={config.title} description={config.description} />
      <PrimaryLayout>
        <Sticky>
          <div>
            <h4>{config.title}</h4>
            <p>Optimizing your Reddit reading experience.</p>
          </div>
        </Sticky>
        <Main noInlinePadding>
          <LoginCard
            title="Beta Access"
            primaryLinkUrl="/login"
            primaryLinkChildren={
              <>
                Login <FontAwesomeIcon size="lg" icon={faArrowRight} />
              </>
            }
          >
            {WhitelistMessage}
          </LoginCard>
        </Main>
      </PrimaryLayout>
    </>
  );
}
