"use client";

import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import IconButton from "../iconButton";
import { useRouter } from "next/navigation";
import Link from "next/link";

export interface BackButtonProps {
  link?: string;
  title?: string;
}

export default function BackButton({ link, title = "Back" }: BackButtonProps) {
  const router = useRouter();

  if (link !== undefined) {
    return (
      <Link href={link}>
        <IconButton icon={faArrowLeft} title={title} />
      </Link>
    );
  }

  return (
    <IconButton
      icon={faArrowLeft}
      title={title}
      onClick={() => router.back()}
    />
  );
}
