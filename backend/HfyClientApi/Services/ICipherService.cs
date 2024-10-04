using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface ICipherService
  {
    string Encrypt(string text);
    Result<string> Decrypt(string text);
  }
}
