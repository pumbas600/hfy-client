import NextLink from "next/link";
import styles from "./links.module.css";
import iconButtonStyles from "../iconButton/iconButton.module.css";
import { ReactNode } from "react";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { cx } from "@/util/classNames";

export interface BaseLinkProps {
  className?: string;
  children?: ReactNode;
  href: string;
  title?: string;
  target?: string;
  icon?: IconProp;
  variant?: "subtle" | "underlined" | "button" | "largeButton" | "iconButton";
  newTab?: boolean;
}

export default function Link({
  className,
  children,
  variant = "underlined",
  icon,
  newTab = true,
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
    case "largeButton":
      variantClassName = `button ${styles.largeButtonLink}`;
      break;
    case "iconButton":
      variantClassName = `button ${styles.buttonLink} ${iconButtonStyles.iconButton} ${iconButtonStyles.primary}`;
      if (icon) {
        children = (
          <>
            <FontAwesomeIcon icon={icon} size="xl" />
            {children}
          </>
        );
      }
  }

  if (props.href?.startsWith("/")) {
    return (
      <NextLink {...props} className={cx(variantClassName, className)}>
        {children}
      </NextLink>
    );
  }

  return (
    <a
      target={newTab ? "_blank" : undefined}
      {...props}
      className={cx(variantClassName, className)}
    >
      {children}
    </a>
  );
}
