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
    image = "/og-image.png";
    imageAlt = config.title;
  }

  return (
    <Head>
      <title>{title}</title>
      <meta name="og:title" content={title} />
      <meta name="twitter:title" content={title} />
      <meta name="twitter:card" content="summary_large_image" />
      <meta name="og:site_name" content={config.title} />
      {/* Unfortunately, we can't use CSS variables so these need to be hardcoded. */}
      <meta
        name="theme-color"
        media="(prefers-color-scheme: light)"
        content="#06402b"
        key="theme-color-light"
      />
      <meta
        name="theme-color"
        media="(prefers-color-scheme: dark)"
        content="#127551"
        key="theme-color-dark"
      />

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
          <meta name="twitter:image" content={image} />
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
