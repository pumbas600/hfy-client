import { HeadMeta } from "@/components/atomic";
import BackButton from "@/components/composite/backButton";
import {
  Aside,
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
        <Sticky start={<BackButton />} />
        <Header>
          <h2>About</h2>
        </Header>
        <Aside />
        <Main>
          <TextLayout>
            <h3>{config.title}</h3>
            <p>
              {config.title} is a simplified Reddit client optimized for reading
              stories on the{" "}
              <a href="https://www.reddit.com/r/HFY/" target="_blank">
                r/HFY
              </a>{" "}
              subreddit. Being designed for stories, it allows for an improved
              user experience and story-specific features.
            </p>
            <p>
              This project is{" "}
              <a href={config.githubUrl} target="_blank">
                open source
              </a>
              . Any and all contributions are welcome!
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
                <a href="https://www.royalroad.com/home" target="_blank">
                  Royal Road
                </a>{" "}
                link, if available.
              </li>
            </ul>
            <p>
              When chapters are processed, missing links from chapters are
              automatically added where possible. When a broken link is found
              (such as a link to a chapter, that doesn’t link back to it) then a
              warning is raised, but currently, <strong>nothing is done</strong>
              . Any other links to Reddit posts are changed to instead link to
              this website.
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
              Originally the aim of this project was to be a full Reddit Client,
              including the ability to upvote, leave comments, and so on.
              Unfortunately, due to the free Reddit API ratelimits this isn’t
              possible. Instead, the requests that can be made to the Reddit API
              are spent processing new posts, and regularly checking old posts
              for new updates.
            </p>
          </TextLayout>
        </Main>
      </PageLayout>
    </>
  );
}
