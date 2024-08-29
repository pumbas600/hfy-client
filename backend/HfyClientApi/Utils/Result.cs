using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Utils
{
  /// <summary>
  /// This Result, unlike the generic version defines a Result with no data. I.e. for a void
  /// return type.
  /// </summary>
  public class Result
  {
    public Error Error { get; }
    public bool IsSuccess => ReferenceEquals(Error, Error.None);
    public bool IsFailure => !IsSuccess;

    protected Result(Error error)
    {
      Error = error;
    }

    public R Match<R>(Func<R> onSuccess, Func<Error, R> onFailure)
    {
      return IsSuccess ? onSuccess() : onFailure(Error);
    }

    public ActionResult ToActionResult(Func<ActionResult> onSuccess)
    {
      return Match(onSuccess, error => error.ToActionResult());
    }

    public static Result Success() => new(Error.None);
    public static Result<T> Success<T>(T data) => new(data, Error.None);
    public static Result<T> Failure<T>(Error error) => new(default, error);

    public static implicit operator Result(Error error) => new(error);
  }

  public class Result<T> : Result
  {
    private readonly T? _data;

    public T Data
    {
      get
      {
        if (IsFailure)
        {
          throw new InvalidOperationException("Data is not available for failed results");
        };

        return _data!;

      }
    }

    internal Result(T? data, Error error) : base(error)
    {
      _data = data;
    }

    public Result<R> Map<R>(Func<T, R> mapper)
    {
      return IsSuccess ? Success(mapper(Data)) : Failure<R>(Error);
    }


    public R Match<R>(Func<T, R> onSuccess, Func<Error, R> onFailure)
    {
      return IsSuccess ? onSuccess(Data) : onFailure(Error);
    }

    public ActionResult ToActionResult(Func<T, ActionResult> onSuccess)
    {
      return Match(onSuccess, error => error.ToActionResult());
    }

    public static implicit operator Result<T>(T data) => Success(data);
    public static implicit operator Result<T>(Error error) => Failure<T>(error);
  }
}
