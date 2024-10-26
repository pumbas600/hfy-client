import config from "@/config";
import Head from "next/head";
import { Fragment, ReactNode } from "react";

export interface HeadMetaProps {
  title: string;
  description?: string | null;
  image?: string | null;
  twitterImage?: string | null;
  imageAlt?: string | null;
  children?: ReactNode;
}

export default function HeadMeta({
  title,
  description,
  image,
  twitterImage,
  imageAlt,
  children,
}: HeadMetaProps) {
  if (!image) {
    image = "/og-image.png";
    twitterImage = "/twitter-image.png";
    imageAlt = config.title;
  }

  if (!twitterImage) {
    twitterImage = image;
  }

  return (
    <Head>
      <title>{title}</title>
      <meta name="og:title" content={title} />
      <meta name="twitter:title" content={title} />
      <meta name="twitter:card" content="summary" />
      <meta name="og:site_name" content={config.title} />

      {description && (
        <>
          <meta name="description" content={description} />
          <meta name="og:description" content={description} />
          <meta name="twitter:description" content={description} />
        </>
      )}

      {image && (
        <>
          <meta name="og:image" content={image} />
          <meta name="twitter:image" content={twitterImage} />
          {imageAlt && <meta name="og:image:alt" content={imageAlt} />}
          {imageAlt && <meta name="twitter:image:alt" content={imageAlt} />}
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
