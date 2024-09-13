"use client";

import { useRouter } from "next/navigation";
import React, { useRef } from "react";
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
  const queryRef = useRef<string>("");
  const router = useRouter();

  const handleQuery = (searchQuery?: string): void => {
    if (searchQuery === undefined || searchQuery === "") {
      router.push(`/r/${subreddit}`);
      return;
    }

    const searchUrl = `/r/${subreddit}?q=${searchQuery}`;
    router.push(searchUrl);
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>): void => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const searchQuery = formData.get("q")?.toString().trim();
    handleQuery(searchQuery);
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const searchQuery = event.target.value;
    const lengthDiff = searchQuery.length - queryRef.current.length;

    if (Math.abs(lengthDiff) >= 2) {
      handleQuery(searchQuery.trim());
    }

    queryRef.current = searchQuery;
  };

  return (
    <form role="search" onSubmit={handleSubmit}>
      <input
        type="search"
        name="q"
        placeholder="Search chapters..."
        aria-label="Search for chapters by their title"
        onChange={handleChange}
        {...props}
        className={`${styles.searchInput} ${props.className ?? ""}`}
      />
    </form>
  );
}
