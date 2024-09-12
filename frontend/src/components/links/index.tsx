import Link from "next/link";
import styles from "./links.module.css";

export interface BaseLinkProps {
  href: string;
  children?: React.ReactNode;
  className?: string;
}

export default function CreateBaseLink(variantClassName: string) {
  return function BaseLink({ className, ...props }: BaseLinkProps) {
    if (props.href.startsWith("/")) {
      return (
        <Link {...props} className={`${variantClassName} ${className ?? ""}`} />
      );
    }

    return (
      <a {...props} className={`${variantClassName} ${className ?? ""}`} />
    );
  };
}

export const SubtleLink = CreateBaseLink(styles.subtleLink);

export const UnderlinedLink = CreateBaseLink(styles.underlinedLink);
