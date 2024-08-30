import ChapterButtons from "@/components/buttons/chapterButtons";
import config from "@/config";
import { ChapterApi } from "@/types/api";
import { Params } from "@/types/next";

export default async function Page({ params }: Params<{ id: string }>) {
  const res = await fetch(`${config.api.baseUrl}/chapters/${params.id}`);
  const chapter: ChapterApi.Res = await res.json();

  if (!res.ok) {
    return (
      <div>
        <h2>Error!</h2>
        <p>{JSON.stringify(chapter, undefined, 4)}</p>
      </div>
    );
  }

  return (
    <article>
      <header>
        <h2>{chapter.title}</h2>
        <a href={chapter.redditPostLink}>Read on Reddit</a>
        <p>r/{chapter.subreddit}</p>
        <p>{chapter.author}</p>
        <time>{/*Created at ...*/}</time>
      </header>
      <ChapterButtons chapter={chapter} />
      <main dangerouslySetInnerHTML={{ __html: chapter.textHtml }} />
      <ChapterButtons chapter={chapter} hideFirstLink />
    </article>
  );
}
