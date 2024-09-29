import { User } from "@/types/user";
import styles from "./profilePicture.module.css";

export interface ProfilePictureProps {
  user: User;
}

export default function ProfilePicture({ user: userDto }: ProfilePictureProps) {
  return (
    <img
      src={userDto.iconUrl}
      alt={userDto.name}
      className={styles.profilePicture}
    />
  );
}
