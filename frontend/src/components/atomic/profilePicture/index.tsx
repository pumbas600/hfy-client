import { UserDto } from "@/types/user";
import styles from "./profilePicture.module.css";

export interface ProfilePictureProps {
  userDto: UserDto;
}

export default function ProfilePicture({ userDto }: ProfilePictureProps) {
  return (
    <img
      src={userDto.iconUrl}
      alt={userDto.name}
      className={styles.profilePicture}
    />
  );
}
