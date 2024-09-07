export interface SearchInputProps {
  placeholder: string;
}

export default function SearchInput({ placeholder }: SearchInputProps) {
  return (
    <div>
      <input type="text" placeholder={placeholder} />
    </div>
  );
}
