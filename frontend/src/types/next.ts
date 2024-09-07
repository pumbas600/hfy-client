export type Params<
  TPathParams extends Record<string, string>,
  TSearchParams extends Record<string, string> = never
> = {
  params: TPathParams;
  searchParams: TSearchParams;
};
export type ReactChildren = { children?: React.ReactNode };
