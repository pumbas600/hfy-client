import { ButtonHTMLAttributes } from "react";
import styles from "./button.module.css";
import { cx } from "@/util/classNames";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "primary" | "subtle";
}

export default function Button({
  className,
  variant = "primary",
  ...props
}: ButtonProps) {
  let variantClassName = "";
  switch (variant) {
    case "subtle":
      variantClassName = styles.subtleButton;
      break;
  }

  return <button {...props} className={cx(variantClassName, className)} />;
}
