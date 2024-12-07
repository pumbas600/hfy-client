using System.Linq.Expressions;

namespace HfyClientApi.Extensions
{
  public static class OrmExtensions
  {
    private class JoinPair<TOuter, TInner>
    {
      public required TOuter OuterValue { get; set; }
      public required IEnumerable<TInner> InnerValue { get; set; }
    }

    public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
      this IQueryable<TOuter> outer, IEnumerable<TInner> inner,
      Expression<Func<TOuter, TKey>> outerKeySelector,
      Expression<Func<TInner, TKey>> innerKeySelector,
      Func<TOuter, TInner?, TResult> resultSelector)
    {
      return outer.GroupJoin(
          inner, outerKeySelector, innerKeySelector,
          (outerValue, innerValue) => new JoinPair<TOuter, TInner>() { OuterValue = outerValue, InnerValue = innerValue })
        .SelectMany(
          x => x.InnerValue.DefaultIfEmpty(),
          (pair, inner) => resultSelector(pair.OuterValue, inner)
        );
    }
  }
}
