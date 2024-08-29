import config from "@/config";
import { ChapterApi } from "@/types/api";
import { Params } from "@/types/next";
import Link from "next/link";

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
      <h2>{chapter.title}</h2>
      <time>{/*Created at ...*/}</time>
      <main dangerouslySetInnerHTML={{ __html: chapter.textHtml }} />
      <div>
        {/* TODO: Make these buttons and disable them if id is null */}
        {chapter.firstChapterId && (
          <Link href={`/chapters/${chapter.firstChapterId}`}>First</Link>
        )}

        {chapter.previousChapterId && (
          <Link href={`/chapters/${chapter.previousChapterId}`}>Previous</Link>
        )}
        {chapter.nextChapterId && (
          <Link href={`/chapters/${chapter.nextChapterId}`}>Next</Link>
        )}
      </div>
    </article>
  );
}
