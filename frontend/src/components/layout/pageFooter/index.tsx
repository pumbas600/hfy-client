import Container from "@/components/container";
import styles from "./pageFooter.module.css";
import config from "@/config";
import { SubtleLink } from "@/components/links";

export default function PageFooter() {
  return (
    <footer className={styles.pageFooter}>
      <Container>
        <p>Hello there :)</p>
        <SubtleLink href={"/"}>Home</SubtleLink>
        <SubtleLink href={"/about"}>About</SubtleLink>
        <SubtleLink href={config.githubUrl}>View source</SubtleLink>
      </Container>
    </footer>
  );
}
