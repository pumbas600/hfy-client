import Container from "@/components/atomic/container";
import styles from "./pageFooter.module.css";
import config from "@/config";
import { Link } from "@/components/atomic";

export default function PageFooter() {
  return (
    <footer className={styles.pageFooter}>
      <Container>
        <p>Hello there :)</p>
        <Link variant="subtle" href={"/"}>
          Home
        </Link>
        <Link variant="subtle" href={"/about"}>
          About
        </Link>
        <Link variant="subtle" href={config.githubUrl}>
          View source
        </Link>
      </Container>
    </footer>
  );
}
