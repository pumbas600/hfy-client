namespace HfyClientApi.Dtos
{
  public class PaginationDto<T>
  {
    public required int Page { get; set; }
    public required int PageSize { get; set; }
    public required IEnumerable<T> Data { get; set; }
  }
}
