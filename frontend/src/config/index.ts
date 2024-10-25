function requiredEnv(key: keyof NodeJS.ProcessEnv): string {
  const value = process.env[key];
  if (!value) {
    throw new Error(`Required environment variable is missing: ${key}`);
  }

  return value;
}

function determineFrontendBaseUrl(): string {
  if (process.env.NEXT_PUBLIC_FRONTEND_URL) {
    return process.env.NEXT_PUBLIC_FRONTEND_URL;
  }

  if (process.env.NEXT_PUBLIC_VERCEL_ENV === "production") {
    return `https://${requiredEnv("NEXT_PUBLIC_VERCEL_URL")}`;
  }

  return "http://localhost:3000";
}

const config = {
  api: {
    baseUrl:
      process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5070/api/v1",
  },
  fontendBaseUrl: determineFrontendBaseUrl(),
  githubUrl: "https://github.com/pumbas600/hfy-client",
  discordInviteUrl: "https://discord.com",
  title: "HFY Client",
  description:
    "HFY Client is a free, open-source client for Reddit designed to optimize your reading experience.",
  cookies: {
    refreshToken: "RefreshToken",
    accessToken: "AccessToken",
    userProfile: "UserProfile",
  },
} as const;

export default config;
