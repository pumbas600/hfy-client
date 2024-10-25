import AppIcon, { AppIconProps } from "@/icons/AppIcon";
import Link from "next/link";

export default function HomeIcon(props: AppIconProps) {
  return (
    <Link href="/">
      <AppIcon {...props} />
    </Link>
  );
}
