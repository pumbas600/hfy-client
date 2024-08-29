import { Params } from "@/types/next";

export default function Page({ params }: Params<{ id: string }>) {
  return (
    <article>
      <h2>Chapter {params.id}</h2>
      <time>{/*Created at ...*/}</time>
      <main>Hello there General Kenobi</main>
      <div>
        <a href="/chapters/first">First</a>
        <a href="/chapters/prev">Prev</a>
        <a href="/chapters/next">Next</a>
      </div>
    </article>
  );
}
