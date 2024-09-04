import Link from "next/link";
import styles from "./links.module.css";

export interface BaseLinkProps {
  href: string;
  children?: React.ReactNode;
}

export default function CreateBaseLink(className: string) {
  return function BaseLink(props: BaseLinkProps) {
    if (props.href.startsWith("/")) {
      return <Link {...props} className={className} />;
    }

    return <a {...props} className={className} />;
  };
}

export const SubtleLink = CreateBaseLink(styles.subtleLink);

export const UnderlinedLink = CreateBaseLink(styles.underlinedLink);
