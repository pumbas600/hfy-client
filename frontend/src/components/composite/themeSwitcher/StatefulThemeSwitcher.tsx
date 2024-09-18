"use client";

import { IconButton } from "@/components/atomic";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { faMoon } from "@fortawesome/free-solid-svg-icons/faMoon";
import { faSun } from "@fortawesome/free-solid-svg-icons/faSun";
import { faCog } from "@fortawesome/free-solid-svg-icons/faCog";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Menu, MenuItem, MenuModifiers } from "@szhsin/react-menu";
import styles from "./themeSwitcher.module.css";
import React, { useMemo, useState } from "react";
import { ObjectUtils } from "@/util/object";
import { setCookie } from "cookies-next";

export type Theme = "light" | "dark" | "system";
export type ResolvedTheme = Exclude<Theme, "system">;

export const DEFAULT_RESOLVED_THEME: ResolvedTheme = "dark";
export const THEME_KEY = "theme";

const ThemeIcons: Record<Theme, IconProp> = {
  light: faSun,
  dark: faMoon,
  system: faCog,
};

function capitalise(string: string): string {
  return string.charAt(0).toUpperCase() + string.slice(1);
}

const IS_SERVER = typeof window === "undefined";

function resolveTheme(theme: Theme): ResolvedTheme {
  if (theme === "system") {
    if (IS_SERVER) {
      // We can't detect the system theme on the server, so default to dark
      return DEFAULT_RESOLVED_THEME;
    }

    theme = window.matchMedia("(prefers-color-scheme: dark)").matches
      ? "dark"
      : "light";
  }

  setCookie(THEME_KEY, theme);
  document.documentElement.setAttribute("data-theme", theme);
  return theme;
}

export interface ThemeSwitcherProps {
  setThemeCookieServerAction(theme: ResolvedTheme): Promise<void>;
}

export default function StatefulThemeSwitcher({
  setThemeCookieServerAction,
}: ThemeSwitcherProps) {
  const [selectedTheme, setSelectedTheme] = useState<Theme>(() => {
    if (!IS_SERVER) {
      const storedTheme = localStorage.getItem(THEME_KEY);
      if (storedTheme) {
        return storedTheme as Theme;
      }
    }

    return "system";
  });

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

  const handleThemeChange = (theme: Theme) => {
    console.log(theme);

    setSelectedTheme(theme);
    setThemeCookieServerAction(resolveTheme(theme));
  };

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
          onClick={() => handleThemeChange(theme)}
          className={theme == selectedTheme ? styles.selected : ""}
        >
          <FontAwesomeIcon icon={icon} size="lg" />
          {capitalise(theme)}
        </MenuItem>
      ))}
    </Menu>
  );
}
