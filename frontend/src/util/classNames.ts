export function cx(...classNames: (string | undefined)[]): string {
  return classNames.filter(Boolean).join(" ");
}
