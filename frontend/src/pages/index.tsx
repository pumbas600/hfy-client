import { HeadMeta, Text } from "@/components/atomic";
import { Main, Sticky } from "@/components/layout/pageLayout";
import PrimaryLayout from "@/components/layout/primaryLayout";
import config from "@/config";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons/faArrowRight";
import { WhitelistMessage } from "@/config/constants";
import LoginCard from "@/components/loginAndAuthorize/loginCard";
import { getSelf } from "@/lib/getSelf";
import NoSsr from "@/components/atomic/NoSsr";

export default function Home() {
  const isAuthenticated = getSelf() !== undefined;

  return (
    <>
      <HeadMeta title={config.title} description={config.description} />
      <PrimaryLayout>
        <Sticky />
        <Main noInlinePadding>
          <NoSsr>
            {isAuthenticated ? (
              <LoginCard
                title="Start Reading"
                primaryLinkUrl="/r/HFY"
                primaryLinkChildren={
                  <>
                    Go to r/HFY{" "}
                    <FontAwesomeIcon size="lg" icon={faArrowRight} />
                  </>
                }
              >
                Welcome to the {config.title} beta! Go to r/HFY to start
                reading.
              </LoginCard>
            ) : (
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
            )}
          </NoSsr>
        </Main>
      </PrimaryLayout>
    </>
  );
}
