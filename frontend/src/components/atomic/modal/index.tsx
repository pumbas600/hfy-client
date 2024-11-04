import { forwardRef, MouseEvent, ReactNode } from "react";
import styles from "./modal.module.css";
import { cx } from "@/util/classNames";

interface ModalProps {
  onClose: () => void;
  children: ReactNode;
  className?: string;
}

const Modal = forwardRef<HTMLDialogElement, ModalProps>(
  ({ onClose, children, className }, ref) => {
    const handleClick = (e: MouseEvent) => {
      if (ref && "current" in ref && ref.current === e.target) {
        onClose();
      }
    };

    return (
      <dialog
        ref={ref}
        onClose={onClose}
        onClick={handleClick}
        className={styles.modal}
      >
        <div className={cx(styles.modalContent, className)}>{children}</div>
      </dialog>
    );
  }
);

Modal.displayName = "Modal";

export default Modal;
