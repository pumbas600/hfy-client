import { Params } from "@/types/next";

export default async function Page({ params }: Params<{ subreddit: string }>) {
  return (
    <main>
      <h1>{params.subreddit}</h1>
    </main>
  );
}
