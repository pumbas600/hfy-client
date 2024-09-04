import {
  FontAwesomeIcon,
  FontAwesomeIconProps,
} from "@fortawesome/react-fontawesome";
import styles from "./iconButton.module.css";

export interface IconButtonProps extends FontAwesomeIconProps {
  onClick?: VoidFunction;
  title: string;
}

export default function IconButton({
  onClick,
  title,
  ...props
}: IconButtonProps) {
  return (
    <button onClick={onClick} title={title} className={styles.iconButton}>
      <FontAwesomeIcon {...props} size={props.size ?? "xl"} />
    </button>
  );
}
