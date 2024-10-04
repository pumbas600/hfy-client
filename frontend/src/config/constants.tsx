import { Link } from "@/components/atomic";
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

export const WhitelistMessage = (
  <>
    {config.title} is currently in beta, with a whitelist of users allowed to
    access it. If you're interested in joining the beta or staying up to date
    with development, feel free join the{" "}
    <Link href={config.discordInviteUrl} variant="subtle">
      Discord
    </Link>
    .
  </>
);
