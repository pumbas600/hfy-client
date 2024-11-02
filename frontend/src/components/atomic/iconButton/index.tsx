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
}

const IconButton = forwardRef<HTMLButtonElement, IconButtonProps>(
  ({ icon, size, ...props }, ref) => {
    return (
      <button
        ref={ref}
        {...props}
        className={cx(styles.iconButton, props.className)}
      >
        <FontAwesomeIcon icon={icon} size={size ?? "xl"} />
      </button>
    );
  }
);

IconButton.displayName = "IconButton";

export default IconButton;
