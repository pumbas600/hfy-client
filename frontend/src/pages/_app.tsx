import { AppProps } from "next/app";

import "@/styles/styles.css";

// See: https://docs.fontawesome.com/web/use-with/react/use-with#getting-font-awesome-css-to-work
import { config as fontAwesomeConfig } from "@fortawesome/fontawesome-svg-core";
import "@fortawesome/fontawesome-svg-core/styles.css";
fontAwesomeConfig.autoAddCss = false;

export default function App({ Component, pageProps }: AppProps) {
  return <Component {...pageProps} />;
}
