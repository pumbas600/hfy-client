import NextLink from "next/link";
import styles from "./links.module.css";
import iconButtonStyles from "../iconButton/iconButton.module.css";
import { ReactNode } from "react";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { cx } from "@/util/classNames";
import { IconButtonProps } from "../iconButton";

interface BaseLinkProps {
  className?: string;
  children?: ReactNode;
  href: string;
  title?: string;
  newTab?: boolean;
  variant?: "subtle" | "underlined" | "button" | "largeButton";
}

interface IconButtonLinkProps extends Omit<BaseLinkProps, "variant"> {
  variant: "iconButton";
  icon: IconProp;
  type: IconButtonProps["variant"];
}

type LinkProps = BaseLinkProps | IconButtonLinkProps;

export default function Link({
  className,
  children,
  newTab = true,
  ...props
}: LinkProps) {
  const linkProps = { href: props.href, title: props.title };

  const classNames = [className];
  if (props.variant === "subtle" || props.variant === undefined) {
    classNames.push(styles.subtleLink);
  } else if (props.variant === "underlined") {
    classNames.push(styles.underlinedLink);
  } else if (props.variant === "button") {
    classNames.push("button", styles.buttonLink);
  } else if (props.variant === "largeButton") {
    classNames.push("button", styles.largeButtonLink);
  } else if (props.variant === "iconButton") {
    classNames.push(
      "button",
      styles.buttonLink,
      iconButtonStyles.iconButton,
      iconButtonStyles[props.type ?? "primary"]
    );

    if (props.icon) {
      children = (
        <>
          <FontAwesomeIcon icon={props.icon} size="xl" />
          {children}
        </>
      );
    }
  }

  if (props.href?.startsWith("/")) {
    return (
      <NextLink {...linkProps} className={cx(...classNames)}>
        {children}
      </NextLink>
    );
  }

  return (
    <a
      target={newTab ? "_blank" : undefined}
      {...linkProps}
      className={cx(...classNames)}
    >
      {children}
    </a>
  );
}
