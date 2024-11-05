import { HeadMeta, Link } from "@/components/atomic";
import HomeIcon from "@/components/composite/homeIcon";
import {
  Aside,
  Footer,
  Header,
  Main,
  PageLayout,
  Sticky,
} from "@/components/layout/pageLayout";
import TextLayout from "@/components/layout/textLayout";
import config from "@/config";

export default function AboutPage() {
  return (
    <>
      <HeadMeta
        title={`About | ${config.title}`}
        description={config.description}
      />
      <PageLayout>
        <Sticky start={<HomeIcon inverted />} />
        <Header>
          <h2>About</h2>
        </Header>
        <Aside />
        <Main>
          <TextLayout>
            <h3>{config.title}</h3>
            <p>
              {config.title} is a simplified client for Reddit, designed for
              reading stories on the{" "}
              <Link href="https://www.reddit.com/r/HFY/">r/HFY</Link> subreddit.
            </p>
            <p>
              This project is free and{" "}
              <Link href={config.githubUrl}>open source</Link>. Any and all
              contributions are welcome!
            </p>
            <h3>Behind the scenes</h3>
            <p>
              {config.title} is designed to be as frictionless for authors as
              possible. It automatically parses HFY Reddit posts so that authors
              can continue posting on Reddit as normal. To achieve this, certain
              metadata is extracted from Reddit posts:
            </p>
            <ul>
              <li>
                <strong>First link</strong> — This is extracted from a link to a
                Reddit post with <em>first</em> in the label. The first link is
                used to quickly determines whether chapters belong to the same
                story.
              </li>
              <li>
                <strong>Next/previous links</strong> — Links to Reddit posts
                with <em>next</em> and <em>prev</em> in the label. These links
                form a linked list of chapters in the same story.
              </li>
              <li>
                <strong>Cover art</strong> — Comes from a link to an image with{" "}
                <em>cover</em> in the label. If that cannot be found, it instead
                gets the image from a{" "}
                <Link href="https://www.royalroad.com/home">Royal Road</Link>{" "}
                link, if available.
              </li>
            </ul>
            <p>
              When chapters are processed, missing links from chapters are
              automatically added where possible. Any other links to Reddit
              posts are automatically updated to link back to this website.
            </p>
            <p>
              Periodically, chapters are synchronised with their corresponding
              Reddit post. The time between synchronisations is proportional to
              how old the post is. As a result, any upvotes or edits to the
              original Reddit post—including removing it all together—get
              reflected here.
            </p>
            <h3>Limitations</h3>
            <p>
              Originally the aim of this project was to fully match the typical
              functionality of Reddit, including the ability to upvote, leave
              comments, and so on. Unfortunately, due to the free Reddit API
              ratelimits this isn’t possible. Instead, the available requests
              are spent processing new posts, and regularly checking old posts
              for new updates.
            </p>
            <p>
              If you have any ideas on how typical Reddit functionality can be
              balanced with their ratelimit, feel free to let me know on the{" "}
              <Link href={config.discordInviteUrl}>Discord</Link>.
            </p>
          </TextLayout>
        </Main>
        <Footer />
      </PageLayout>
    </>
  );
}
