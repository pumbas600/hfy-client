using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Utils
{
  public class Error
  {
    public static readonly Error None = new("None", "No error", HttpStatusCode.OK);

    public string Code { get; }
    public string Message { get; }
    public int StatusCode { get; }

    public Error(string code, string message, int statusCode)
    {
      Code = code;
      Message = message;
      StatusCode = statusCode;
    }

    public Error(string code, string message, HttpStatusCode statusCode)
        : this(code, message, (int)statusCode)
    {
    }

    public ActionResult ToActionResult()
    {
      return new ObjectResult(new { Code, Message })
      {
        StatusCode = StatusCode
      };
    }
  }
}
