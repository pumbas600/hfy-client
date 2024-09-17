"use client";

import { IconButton } from "@/components/atomic";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { faMoon } from "@fortawesome/free-solid-svg-icons/faMoon";
import { faSun } from "@fortawesome/free-solid-svg-icons/faSun";
import { faCog } from "@fortawesome/free-solid-svg-icons/faCog";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Menu, MenuItem } from "@szhsin/react-menu";
import styles from "./themeSwitcher.module.css";

type Theme = "light" | "dark" | "system";

const ThemeIcons: Record<Theme, IconProp> = {
  light: faSun,
  dark: faMoon,
  system: faCog,
};

function capitalise(string: string): string {
  return string.charAt(0).toUpperCase() + string.slice(1);
}

export default function ThemeSwitcher() {
  const selectedTheme: Theme = "light";
  const resolvedTheme: Exclude<Theme, "system"> = selectedTheme;

  return (
    <Menu
      menuClassName={styles.menu}
      menuButton={
        <IconButton icon={ThemeIcons[resolvedTheme]} title="Select theme" />
      }
      transition
    >
      {Object.entries(ThemeIcons).map(([theme, icon]) => (
        <MenuItem
          key={theme}
          className={theme == selectedTheme ? styles.selected : ""}
        >
          <FontAwesomeIcon icon={icon} size="lg" />
          {capitalise(theme)}
        </MenuItem>
      ))}
    </Menu>
  );
}
