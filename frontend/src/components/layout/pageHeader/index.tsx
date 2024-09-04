import Container, { ContainerProps } from "@/components/container";
import styles from "./pageHeader.module.css";

export default function PageHeader(containerProps: ContainerProps) {
  return (
    <header className={styles.pageHeader}>
      <Container {...containerProps} />
    </header>
  );
}
