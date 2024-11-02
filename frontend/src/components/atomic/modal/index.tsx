import { forwardRef, ReactNode } from "react";
import styles from "./modal.module.css";

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  children: ReactNode;
}

const Modal = forwardRef<HTMLDialogElement, ModalProps>(
  ({ isOpen, onClose, children }, ref) => {
    return (
      <dialog
        ref={ref}
        open={isOpen}
        onClose={onClose}
        className={styles.modal}
      >
        <button>Hello</button>
        {children}
      </dialog>
    );
  }
);

export default Modal;
