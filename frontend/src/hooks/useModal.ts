import { RefObject, useCallback, useRef } from "react";

interface UseModalReturnValue {
  modalRef: RefObject<HTMLDialogElement>;
  open: () => void;
  close: () => void;
}

export default function useModal(): UseModalReturnValue {
  const modalRef = useRef<HTMLDialogElement>(null);

  const open = useCallback(() => {
    modalRef.current?.showModal();
  }, []);

  const close = useCallback(() => {
    modalRef.current?.close();
  }, []);

  return { modalRef, open, close };
}
