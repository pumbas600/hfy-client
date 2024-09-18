import { cookies } from "next/headers";
import StatefulThemeSwitcher, { ResolvedTheme } from "./StatefulThemeSwitcher";

export async function setCookieServerAction(
  theme: ResolvedTheme
): Promise<void> {
  "use server";

  const cookieStore = cookies();
  cookieStore.set("theme", theme, {
    path: "/",
    maxAge: 60 * 60 * 24 * 365,
  });
}

export default function ThemeSwitcher() {
  return (
    <StatefulThemeSwitcher setThemeCookieServerAction={setCookieServerAction} />
  );
}
