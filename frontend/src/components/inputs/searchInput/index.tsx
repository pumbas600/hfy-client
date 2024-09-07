import { DetailedHTMLProps, InputHTMLAttributes } from "react";

type SearchInputProps = DetailedHTMLProps<
  InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
>;

export default function SearchInput(props: SearchInputProps) {
  return (
    <form role="search">
      <input type="search" {...props} />
    </form>
  );
}
