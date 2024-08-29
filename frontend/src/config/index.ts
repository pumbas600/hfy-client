function requiredEnv(key: keyof NodeJS.ProcessEnv): string {
  const value = process.env[key];
  if (!value) {
    throw new Error(`Required environment variable is missing: ${key}`);
  }

  return value;
}

const config = {
  api: {
    baseUrl: process.env.API_BASE_URL || "https://localhost:7112",
  },
} as const;

export default config;
