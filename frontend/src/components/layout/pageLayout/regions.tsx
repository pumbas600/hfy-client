import SelfProfile from "@/components/composite/selfProfile";
import styles from "./pageLayout.module.css";
import { User } from "@/types/user";
import { ReactNode } from "react";
import { Button } from "@/components/atomic";

export interface RegionProps {
  className?: string;
  children?: ReactNode;
}

export interface StickyProps {
  start?: ReactNode;
  children?: ReactNode;
  end?: ReactNode;
  self?: User;
  className?: string;
}

export function Sticky({ start, children, end, self, className }: StickyProps) {
  return (
    <div className={`${styles.sticky} ${className ?? ""}`}>
      <div className={styles.stickyContent}>
        {start ?? <div />}
        {children ?? <div />}

        <div className={styles.row}>
          {end}{" "}
          {self && (
            <>
              <SelfProfile key="profile" user={self} />
              <Button variant="subtle">Log out</Button>
            </>
          )}
        </div>
      </div>
    </div>
  );
}

export function Header({ children, className }: RegionProps) {
  return (
    <header className={styles.header}>
      <div className={`${styles.content} ${className ?? ""}`}>{children}</div>
    </header>
  );
}

export interface MainProps extends RegionProps {
  noInlinePadding?: boolean;
}

export function Main({ children, className, noInlinePadding }: MainProps) {
  const mainPaddingClassName = noInlinePadding ? "" : styles.mainPadding;

  return (
    <main
      className={`${styles.main} ${mainPaddingClassName} ${className ?? ""}`}
    >
      {children}
    </main>
  );
}
