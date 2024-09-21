"use client";

import React, { useState } from "react";
import { ResolvedTheme, Theme } from "@/config/getTheme";
import styles from "./themeSelect.module.css";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { faSun } from "@fortawesome/free-solid-svg-icons/faSun";
import { faMoon } from "@fortawesome/free-solid-svg-icons/faMoon";
import { faCog } from "@fortawesome/free-solid-svg-icons/faCog";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export const ThemeKey = "hfy.theme";
const DefaultTheme: Theme = "system";
const DefaultResolvedTheme: ResolvedTheme = "dark";

const ThemeIcons: Record<Theme, IconProp> = {
  light: faSun,
  dark: faMoon,
  system: faCog,
};

const IS_SERVER = typeof window === "undefined";

function resolveTheme(theme: Theme): ResolvedTheme {
  if (theme === "system") {
    if (IS_SERVER) {
      // We can't detect the system theme on the server, so default to dark
      return DefaultResolvedTheme;
    }

    theme = window.matchMedia("(prefers-color-scheme: dark)").matches
      ? "dark"
      : "light";
  }

  localStorage.setItem(ThemeKey, theme);
  document.documentElement.dataset.theme = theme;
  return theme;
}

export default function ThemeSelect() {
  const [selectedTheme, setSelectedTheme] = useState<Theme>(() => {
    if (!IS_SERVER) {
      const storedTheme = localStorage.getItem(ThemeKey);
      if (storedTheme) {
        return storedTheme as Theme;
      }
    }

    return DefaultTheme;
  });

  const handleThemeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const theme = e.target.value as Theme;

    setSelectedTheme(theme);
    resolveTheme(theme);
  };

  return (
    <form className={styles.wrapper}>
      <label id="theme-select-label">Theme</label>
      <div className={styles.themeSelect}>
        <div className={styles.iconWrapper}>
          <FontAwesomeIcon icon={ThemeIcons[selectedTheme]} size="lg" />
        </div>
        <select
          aria-labelledby="theme-select-label"
          name="theme"
          value={selectedTheme}
          onChange={handleThemeChange}
        >
          <option value="light">Light</option>
          <option value="dark">Dark</option>
          <option value="system">System</option>
        </select>
      </div>
    </form>
  );
}
