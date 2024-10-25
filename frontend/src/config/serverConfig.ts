function requiredEnv(key: keyof NodeJS.ProcessEnv): string {
  const value = process.env[key];
  if (!value) {
    throw new Error(`Required environment variable is missing: ${key}`);
  }

  return value;
}

function determineFrontendBaseUrl(): string {
  if (process.env.FRONTEND_BASE_URL) {
    return process.env.FRONTEND_BASE_URL;
  }

  if (process.env.VERCEL_ENV === "production") {
    return `https://${requiredEnv("VERCEL_URL")}`;
  }

  return "http://localhost:3000";
}

const serverConfig = {
  frontendUrl: determineFrontendBaseUrl(),
} as const;

export default serverConfig;
