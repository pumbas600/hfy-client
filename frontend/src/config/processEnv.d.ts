/**
 * Use module augmentation to include the environment variables to the `process.env` object.
 */
declare namespace NodeJS {
  export interface ProcessEnv {
    NEXT_PUBLIC_API_BASE_URL?: string;
    NEXT_PUBLIC_FRONTEND_BASE_URL?: string;

    // Vercel environment variables
    NEXT_PUBLIC_VERCEL_ENV?: string;
    NEXT_PUBLIC_VERCEL_URL?: string;
  }
}
