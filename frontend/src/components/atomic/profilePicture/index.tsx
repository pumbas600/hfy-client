import { User } from "@/types/user";
import styles from "./profilePicture.module.css";

export interface ProfilePictureProps {
  user: User;
  className?: string;
}

export default function ProfilePicture({
  user,
  className,
}: ProfilePictureProps) {
  return (
    <img
      src={user.iconUrl}
      alt={user.name}
      className={`${styles.profilePicture} ${className ?? ""}`}
    />
  );
}