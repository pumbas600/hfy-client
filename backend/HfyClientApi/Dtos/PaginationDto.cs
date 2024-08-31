namespace HfyClientApi.Dtos
{
  /// <summary>
  /// A DTO for paginated data using
  /// <a href="https://learn.microsoft.com/en-us/ef/core/querying/pagination#keyset-pagination">
  /// keyset pagination</a>.
  /// </summary>
  /// <typeparam name="K">The type of the keyset pagination key</typeparam>
  /// <typeparam name="T">The type of the paginated data</typeparam>
  public class PaginationDto<K, T>
  {
    /// <summary>
    /// The keyset pagination key for the next page of data.
    /// </summary>
    public required K NextKey { get; set; }
    public required int PageSize { get; set; }
    public required IEnumerable<T> Data { get; set; }
  }
}
