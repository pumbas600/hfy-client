namespace HfyClientApi.Extensions
{
  public static class CollectionExtensions
  {
    public static V AddIfAbsent<K, V>(this Dictionary<K, V> dict, K key, V value) where K : notnull
    {
      if (dict.TryGetValue(key, out V? existingValue))
      {
        return existingValue;
      }
      dict[key] = value;
      return value;
    }
  }
}
