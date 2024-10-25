/**
 * Use module augmentation to include the environment variables to the `process.env` object.
 */
declare namespace NodeJS {
  export interface ProcessEnv {
    NEXT_PUBLIC_API_BASE_URL?: string;
    FRONTEND_BASE_URL?: string;

    // Vercel environment variables
    VERCEL_URL?: string;
  }
}
