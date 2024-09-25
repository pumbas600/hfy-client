import config from ".";

export const SupportedSubreddits = {
  "r/HFY": "/r/HFY",
} as const;

export const Links = {
  About: "/about",
  Settings: "/settings",
} as const;

export const DevelopmentLinks = {
  "View source": config.githubUrl,
  "Request feature": `${config.githubUrl}/issues`,
  "Report bug": `${config.githubUrl}/issues`, // TODO: Create proper templates!
  "Report security vulnerability": `${config.githubUrl}/issues`,
} as const;
