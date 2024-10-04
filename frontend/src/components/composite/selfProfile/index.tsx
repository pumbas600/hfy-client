import { Link } from "@/components/atomic";
import ProfilePicture from "@/components/atomic/profilePicture";
import { getSelf } from "@/lib/getSelf";
import dynamic from "next/dynamic";

function SelfProfile() {
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

// Prevent SSR to avoid hydration issues.
const DynamicSelfProfile = dynamic(() => Promise.resolve(SelfProfile), {
  ssr: false,
});

export default DynamicSelfProfile;
