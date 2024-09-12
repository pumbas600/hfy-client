import Container from "@/components/atomic/container";
import PageFooter from "@/components/layout/pageFooter";
import PageHeader from "@/components/layout/pageHeader";
import TextLayout from "@/components/layout/textLayout";
import config from "@/config";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: `About | ${config.title}`,
};

export default function About() {
  return (
    <div>
      <PageHeader>
        <h1>About</h1>
      </PageHeader>
      <Container>
        <TextLayout>
          <h3>{config.title}</h3>
          <p>
            {config.title} is a simplified Reddit client optimised for reading
            stories on the <a href="https://www.reddit.com/r/HFY/">r/HFY</a>{" "}
            subreddit. Being designed for stories, it allows for an improved
            user experience and story-specific features.
          </p>
          <p>
            This project is <a href={config.githubUrl}>open source</a>. Any and
            all contributions are welcome!
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
              <strong>Next/previous links</strong> — Links to Reddit posts with{" "}
              <em>next</em> and <em>prev</em> in the label. These links form a
              linked list of chapters in the same story.
            </li>
            <li>
              <strong>Cover art</strong> — Comes from a link to an image with{" "}
              <em>cover</em> in the label. If that cannot be found, it instead
              gets the image from a{" "}
              <a href="https://www.royalroad.com/home">Royal Road</a> link, if
              available.
            </li>
          </ul>
          <p>
            When chapters are processed, missing links from chapters are
            automatically added where possible. When a broken link is found
            (such as a link to a chapter, that doesn’t link back to it) then a
            warning is raised, but currently, <strong>nothing is done</strong>.
            Any other links to Reddit posts are changed to instead link to this
            website.
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
            are spent processing new posts, and regularly checking old posts for
            new updates.
          </p>
        </TextLayout>
      </Container>
      <PageFooter />
    </div>
  );
}
