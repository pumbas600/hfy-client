export type Theme = "light" | "dark" | "system";
export type ResolvedTheme = Exclude<Theme, "system">;

const code = function () {
  // These have to be defined here because the function gets turned into a string.
  const ThemeKey = "hfy.theme";
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

  document.documentElement.dataset.theme = resolvedTheme;
};

export const getTheme = `(${code})();`;
