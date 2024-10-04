using Microsoft.AspNetCore.DataProtection;

namespace HfyClientApi.Services
{
  public class CipherService : ICipherService
  {
    private readonly IDataProtector _protector;

    public CipherService(IDataProtectionProvider dataProtectionProvider)
    {
      _protector = dataProtectionProvider.CreateProtector("HfyClientApi.Services.CipherService");
    }

    public string Decrypt(string text)
    {
      return _protector.Unprotect(text);
    }

    public string Encrypt(string text)
    {
      return _protector.Protect(text);
    }
  }
}
