namespace HfyClientApi.Services
{
  public interface ICipherService
  {
    string Encrypt(string text);
    string Decrypt(string text);
  }
}
