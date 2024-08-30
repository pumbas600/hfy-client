import Container from "@/components/container";
import styles from "./pageFooter.module.css";
import config from "@/config";

export default function PageFooter() {
  return (
    <footer className={styles.pageFooter}>
      <Container>
        <p>Hello there :)</p>
        <a href={config.githubUrl}>View source</a>
      </Container>
    </footer>
  );
}
