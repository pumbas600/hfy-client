import { Link, NoSsr, ProfilePicture } from "@/components/atomic";
import { getSelf } from "@/lib/getSelf";

export default function SelfProfile() {
  const self = getSelf();

  if (!self) {
    return null;
  }

  return (
    <NoSsr>
      <Link variant="iconButton" href="/settings" title="Settings">
        <ProfilePicture user={self} />
      </Link>
    </NoSsr>
  );
}
