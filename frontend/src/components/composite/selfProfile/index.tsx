import ProfilePicture, {
  ProfilePictureProps,
} from "@/components/atomic/profilePicture";
import Link from "next/link";
import styles from "./selfProfile.module.css";

export default function SelfProfile(props: ProfilePictureProps) {
  return (
    <Link href="/settings">
      <ProfilePicture
        {...props}
        className={`${styles.selfProfile} ${props.className ?? ""}`}
      />
    </Link>
  );
}
