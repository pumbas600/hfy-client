import { LocalStorageKeys } from "./localStorage";

export type Theme = "light" | "dark" | "system";
export type ResolvedTheme = Exclude<Theme, "system">;

const code = function () {
  // We replace this string with the actual theme key.
  const ThemeKey = "hfy:theme";
  let theme: Theme = "system";
  let resolvedTheme: ResolvedTheme = "dark";
  try {
    theme = (localStorage.getItem(ThemeKey) ?? "system") as Theme;
  } catch (err) {}

  if (theme == "system") {
    const preferDarkQuery = "(prefers-color-scheme: dark)";
    const mediaQueryList = window.matchMedia(preferDarkQuery);
    const supportsColorSchemeQuery = mediaQueryList.media === preferDarkQuery;

    if (supportsColorSchemeQuery) {
      resolvedTheme = mediaQueryList.matches ? "dark" : "light";
    }
  } else {
    resolvedTheme = theme;
  }

  console.log("[Theme] Resolved theme:", resolvedTheme);
  document.documentElement.dataset.theme = resolvedTheme;
};

export const getTheme = `(${code})();`.replace(
  "hfy:theme",
  LocalStorageKeys.theme
);
