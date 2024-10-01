import { ButtonHTMLAttributes } from "react";
import linkStyles from "../link/links.module.css";
import styles from "./button.module.css";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "primary" | "text";
}

export default function Button({
  className,
  variant = "primary",
  ...props
}: ButtonProps) {
  let variantClassName = "";
  switch (variant) {
    case "text":
      variantClassName = `${styles.textButton} ${linkStyles.subtleLink}`;
      break;
  }

  return (
    <button {...props} className={`${variantClassName} ${className ?? ""}`} />
  );
}
