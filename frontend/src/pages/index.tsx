import { Card, HeadMeta, Link, Text } from "@/components/atomic";
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
            <Text color="primaryContrastSecondary">
              Optimizing your Reddit reading experience.
            </Text>
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
