import { Link } from "@/components/atomic";
import config from ".";

export const Links = {
  Home: "/",
  About: "/about",
} as const;

export const DevelopmentLinks = {
  Contribute: config.githubUrl,
  "Request feature": `${config.githubUrl}/issues`,
  "Report bug": `${config.githubUrl}/issues`, // TODO: Create proper templates!
  "Report security vulnerability": `${config.githubUrl}/issues`,
} as const;

export const WhitelistMessage = (
  <>
    {config.title} is currently in beta, with a whitelist of users allowed to
    access it. If you’re interested in joining the beta or staying up to date
    with development, feel free join the{" "}
    <Link href={config.discordInviteUrl} variant="subtle">
      Discord
    </Link>
    .
  </>
);
