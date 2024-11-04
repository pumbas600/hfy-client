import {
  FontAwesomeIcon,
  FontAwesomeIconProps,
} from "@fortawesome/react-fontawesome";
import { ButtonHTMLAttributes, forwardRef } from "react";
import styles from "./iconButton.module.css";
import { cx } from "@/util/classNames";

export interface IconButtonProps
  extends ButtonHTMLAttributes<HTMLButtonElement> {
  icon: FontAwesomeIconProps["icon"];
  size?: FontAwesomeIconProps["size"];
  variant?: "primary" | "ghost";
}

const IconButton = forwardRef<HTMLButtonElement, IconButtonProps>(
  ({ icon, size, variant, className, ...props }, ref) => {
    const classes = [styles.iconButton, className];
    switch (variant) {
      case "primary":
        classes.push(styles.primary);
        break;
      case "ghost":
        classes.push(styles.ghost);
        break;
    }

    return (
      <button ref={ref} {...props} className={cx(...classes)}>
        <FontAwesomeIcon icon={icon} size={size ?? "xl"} />
      </button>
    );
  }
);

IconButton.displayName = "IconButton";

export default IconButton;
