"use client";

import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { IconButton, Link } from "@/components/atomic";
import { useRouter } from "next/navigation";

export interface BackButtonProps {
  link?: string;
  title?: string;
}

export default function BackButton({ link, title = "Back" }: BackButtonProps) {
  const router = useRouter();

  if (link !== undefined) {
    return (
      <Link variant="iconButton" href={link} title="title" icon={faArrowLeft} />
    );
  }

  // TODO: Look back into the history until /r/hfy

  return (
    <IconButton
      icon={faArrowLeft}
      title={title}
      onClick={() => router.back()}
    />
  );
}
