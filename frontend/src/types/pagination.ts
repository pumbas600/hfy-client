export interface Pagination<TKey, TData> {
  nextKey: TKey | null;
  pageSize: number;
  data: TData[];
}
