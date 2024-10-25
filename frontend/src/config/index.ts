const config = {
  api: {
    baseUrl:
      process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5070/api/v1",
  },
  githubUrl: "https://github.com/pumbas600/hfy-client",
  discordInviteUrl: "https://discord.gg/SuFQXGBG",
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
