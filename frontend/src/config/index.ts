function requiredEnv(key: keyof NodeJS.ProcessEnv): string {
  const value = process.env[key];
  if (!value) {
    throw new Error(`Required environment variable is missing: ${key}`);
  }

  return value;
}

const config = {
  api: {
    baseUrl:
      process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5070/api/v1",
  },
} as const;

export default config;
