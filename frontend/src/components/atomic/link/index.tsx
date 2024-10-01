import NextLink from "next/link";
import styles from "./links.module.css";

export interface BaseLinkProps {
  variant?: "subtle" | "underlined" | "button" | "iconButton";
  children?: React.ReactNode;
  className?: string;
  href: string;
}

export default function Link({
  className,
  variant = "underlined",
  ...props
}: BaseLinkProps) {
  let variantClassName = "";
  switch (variant) {
    case "subtle":
      variantClassName = styles.subtleLink;
      break;
    case "underlined":
      variantClassName = styles.underlinedLink;
      break;
    case "button":
      variantClassName = `button ${styles.buttonLink}`;
      break;
    case "iconButton":
      variantClassName = `button ${styles.iconButtonLink}`;
      break;
  }

  if (props.href.startsWith("/")) {
    return (
      <NextLink
        {...props}
        className={`${variantClassName} ${className ?? ""}`}
      />
    );
  }

  return (
    <a
      target="_blank"
      {...props}
      className={`${variantClassName} ${className ?? ""}`}
    />
  );
}
