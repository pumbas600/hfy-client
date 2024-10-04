using System.Security.Cryptography;
using HfyClientApi.Exceptions;
using HfyClientApi.Utils;
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

    public Result<string> Decrypt(string text)
    {
      try
      {
        return _protector.Unprotect(text);
      }
      catch (CryptographicException)
      {
        return Errors.DecryptMalformedCipherError;
      }
    }

    public string Encrypt(string text)
    {
      return _protector.Protect(text);
    }
  }
}
