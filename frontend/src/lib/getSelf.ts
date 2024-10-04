import config from "@/config";
import { User } from "@/types/user";
import { getCookie } from "cookies-next";

export function getSelf(): User | undefined {
  const userProfile = getCookie(config.cookies.userProfile);

  if (userProfile) {
    try {
      return JSON.parse(Buffer.from(userProfile, "base64").toString()) as User;
    } catch (e) {
      console.error("Failed to extract user profile from cookie", e);
    }
  }

  return undefined;
}
