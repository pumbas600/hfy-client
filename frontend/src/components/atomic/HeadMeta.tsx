import Head from "next/head";
import { ReactNode } from "react";

export interface HeadMetaProps {
  title: string;
  description?: string | null;
  image?: string | null;
  imageAlt?: string | null;
  children?: ReactNode;
}

export default function HeadMeta({
  title,
  description,
  image,
  imageAlt,
  children,
}: HeadMetaProps) {
  return (
    <Head>
      <title>{title}</title>
      <meta name="og:title" content={title} />

      {description && (
        <>
          <meta name="description" content={description} />
          <meta name="og:description" content={description} />
        </>
      )}

      {image && (
        <>
          <meta name="og:image" content={image} />
          {imageAlt && <meta name="og:image:alt" content={imageAlt} />}
        </>
      )}
      <link rel="icon" type="image/x-icon" href="/favicon.ico" />
      {children}
    </Head>
  );
}
