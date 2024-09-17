"use client";

import { IconButton } from "@/components/atomic";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { faMoon } from "@fortawesome/free-solid-svg-icons/faMoon";
import { faSun } from "@fortawesome/free-solid-svg-icons/faSun";
import { faCog } from "@fortawesome/free-solid-svg-icons/faCog";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Menu, MenuItem, MenuModifiers } from "@szhsin/react-menu";
import styles from "./themeSwitcher.module.css";
import { useMemo, useState } from "react";
import { ObjectUtils } from "@/util/object";

type Theme = "light" | "dark" | "system";
type ResolvedTheme = Exclude<Theme, "system">;

const ThemeIcons: Record<Theme, IconProp> = {
  light: faSun,
  dark: faMoon,
  system: faCog,
};

function capitalise(string: string): string {
  return string.charAt(0).toUpperCase() + string.slice(1);
}

function resolveTheme(theme: Theme): ResolvedTheme {
  if (theme !== "system") {
    return theme;
  }

  return window.matchMedia("(prefers-color-scheme: dark)").matches
    ? "dark"
    : "light";
}

export default function ThemeSwitcher() {
  const [selectedTheme, setSelectedTheme] = useState<Theme>("system");
  const resolvedTheme = useMemo(
    () => resolveTheme(selectedTheme),
    [selectedTheme]
  );

  const menuClassName = ({ state }: MenuModifiers) =>
    state === "opening"
      ? styles.menuOpening
      : state === "closing"
      ? styles.menuClosing
      : styles.menu;

  return (
    <Menu
      transition
      menuClassName={menuClassName}
      menuButton={
        <IconButton icon={ThemeIcons[resolvedTheme]} title="Select theme" />
      }
    >
      {ObjectUtils.entries(ThemeIcons).map(([theme, icon]) => (
        <MenuItem
          key={theme}
          className={theme == selectedTheme ? styles.selected : ""}
          onClick={() => setSelectedTheme(theme)}
        >
          <FontAwesomeIcon icon={icon} size="lg" />
          {capitalise(theme)}
        </MenuItem>
      ))}
    </Menu>
  );
}
