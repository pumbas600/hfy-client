"use client";

import { useRouter } from "next/navigation";
import React from "react";
import styles from "./chapterSearchInput.module.css";

export interface CharacterSearchInputProps
  extends React.DetailedHTMLProps<
    React.InputHTMLAttributes<HTMLInputElement>,
    HTMLInputElement
  > {
  subreddit: string;
}

export default function ChapterSearchInput({
  subreddit,
  ...props
}: CharacterSearchInputProps) {
  const router = useRouter();

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>): void => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const searchQuery = formData.get("q")?.toString().trim();

    if (searchQuery === undefined || searchQuery === "") {
      router.push(`/r/${subreddit}`);
      return;
    }

    const searchUrl = `/r/${subreddit}/search?q=${searchQuery}`;
    router.push(searchUrl);
  };

  return (
    <form role="search" onSubmit={handleSubmit}>
      <input
        type="search"
        name="q"
        placeholder="Search chapters..."
        aria-label="Search for chapters by their title"
        {...props}
        className={`${styles.searchInput} ${props.className ?? ""}`}
      />
    </form>
  );
}
