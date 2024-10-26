import config from "@/config";
import Head from "next/head";
import { Fragment, ReactNode } from "react";

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
  if (!image) {
    image = "/light-mode-icon-320x320.png";
    imageAlt = `The ${config.title} logo`;
  }

  return (
    <Head>
      <title>{title}</title>
      <meta name="og:title" content={title} />
      <meta name="twitter:card" content="summary" />

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
      {[16, 32].map((size) => (
        <Fragment key={size}>
          <link
            href={`/light-mode-favicon-${size}x${size}.png`}
            rel="icon"
            type="image/png"
            sizes={`${size}x${size}`}
            media="(prefers-color-scheme: light)"
          />
          <link
            href={`/dark-mode-favicon-${size}x${size}.png`}
            rel="icon"
            type="image/png"
            sizes={`${size}x${size}`}
            media="(prefers-color-scheme: dark)"
          />
        </Fragment>
      ))}

      {children}
    </Head>
  );
}
