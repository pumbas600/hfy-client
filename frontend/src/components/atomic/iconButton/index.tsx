import {
  FontAwesomeIcon,
  FontAwesomeIconProps,
} from "@fortawesome/react-fontawesome";
import { ButtonHTMLAttributes } from "react";
import styles from "./iconButton.module.css";

export interface IconButtonProps
  extends ButtonHTMLAttributes<HTMLButtonElement> {
  icon: FontAwesomeIconProps["icon"];
  size?: FontAwesomeIconProps["size"];
}

export default function IconButton({ icon, size, ...props }: IconButtonProps) {
  return (
    <button
      {...props}
      className={`${styles.iconButton} ${props.className ?? ""}`}
    >
      <FontAwesomeIcon icon={icon} size={size ?? "xl"} />
    </button>
  );
}
