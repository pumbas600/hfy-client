import { forwardRef, ReactNode } from "react";
import styles from "./modal.module.css";

interface ModalProps {
  onClose: () => void;
  children: ReactNode;
}

const Modal = forwardRef<HTMLDialogElement, ModalProps>(
  ({ onClose, children }, ref) => {
    return (
      <dialog ref={ref} onClose={onClose} className={styles.modal}>
        {children}
      </dialog>
    );
  }
);

export default Modal;
