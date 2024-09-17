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
      <MenuItem>
        <FontAwesomeIcon icon={ThemeIcons.light} size="lg" />
        Light
      </MenuItem>
      <MenuItem>
        <FontAwesomeIcon icon={ThemeIcons.dark} size="lg" />
        Dark
      </MenuItem>
      <MenuItem>
        <FontAwesomeIcon icon={ThemeIcons.system} size="lg" />
        System
      </MenuItem>
    </Menu>
  );
}
