import { ButtonHTMLAttributes } from "react";
import linkStyles from "../link/links.module.css";
import styles from "./button.module.css";

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

  return (
    <button {...props} className={`${variantClassName} ${className ?? ""}`} />
  );
}
