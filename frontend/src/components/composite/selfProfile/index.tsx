import { Link } from "@/components/atomic";
import ProfilePicture, {
  ProfilePictureProps,
} from "@/components/atomic/profilePicture";

export default function SelfProfile(props: ProfilePictureProps) {
  return (
    <Link variant="iconButton" href="/settings" title="Settings">
      <ProfilePicture {...props} className={props.className} />
    </Link>
  );
}
