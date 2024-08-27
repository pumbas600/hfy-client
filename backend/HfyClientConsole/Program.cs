using System.Diagnostics;
using HfyClientApi.Configuration;
using Microsoft.Extensions.Configuration;
using Reddit.AuthTokenRetriever;

var configuration = new ConfigurationBuilder()
  .AddUserSecrets<Program>()
  .Build();

Console.WriteLine(configuration[Config.RedditAppId]);

var authTokenRetriever = new AuthTokenRetrieverLib(
  configuration[Config.RedditAppId],
  8080,
  appSecret: configuration[Config.RedditAppSecret]
);

static void OpenBrowser(string authUrl, string browserPath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe")
{
  try
  {
    ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
    Process.Start(processStartInfo);
  }
  catch (System.ComponentModel.Win32Exception)
  {
    // This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
    ProcessStartInfo processStartInfo = new ProcessStartInfo(browserPath)
    {
      Arguments = authUrl
    };
    Process.Start(processStartInfo);
  }
}
