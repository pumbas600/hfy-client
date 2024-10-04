import { Link } from "@/components/atomic";
import ProfilePicture from "@/components/atomic/profilePicture";
import { getSelf } from "@/lib/getSelf";

export default function SelfProfile() {
  const self = getSelf();

  if (!self) {
    return null;
  }

  return (
    <Link variant="iconButton" href="/settings" title="Settings">
      <ProfilePicture user={self} />
    </Link>
  );
}
