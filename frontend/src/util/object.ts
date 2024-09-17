export namespace ObjectUtils {
  export function entries<TKey extends string, TValue>(
    object: Record<TKey, TValue>
  ): [TKey, TValue][] {
    return Object.entries(object) as [TKey, TValue][];
  }
}
