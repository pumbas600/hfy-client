export interface SearchInputProps {
  "aria-label": string;
  placeholder: string;
}

export default function SearchInput(props: SearchInputProps) {
  return (
    <form role="search">
      <input type="search" {...props} />
    </form>
  );
}
