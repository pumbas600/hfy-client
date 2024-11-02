import { forwardRef, ReactNode } from "react";
import styles from "./modal.module.css";
import { cx } from "@/util/classNames";

interface ModalProps {
  onClose: () => void;
  children: ReactNode;
  className?: string;
}

const Modal = forwardRef<HTMLDialogElement, ModalProps>(
  ({ onClose, children, className }, ref) => {
    return (
      <dialog
        ref={ref}
        onClose={onClose}
        className={cx(styles.modal, className)}
      >
        {children}
      </dialog>
    );
  }
);

export default Modal;
