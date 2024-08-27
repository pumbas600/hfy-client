using System.Diagnostics;
using HfyClientApi.Configuration;
using Microsoft.Extensions.Configuration;
using Reddit.AuthTokenRetriever;
using Reddit.AuthTokenRetriever.EventArgs;

var configuration = new ConfigurationBuilder()
  .AddUserSecrets<Program>()
  .Build();

Console.WriteLine(configuration[Config.RedditAppId]);

var authTokenRetriever = new AuthTokenRetrieverLib(
  configuration[Config.RedditAppId],
  8080,
  appSecret: configuration[Config.RedditAppSecret]

);

authTokenRetriever.AuthSuccess += C_AuthSuccess;
authTokenRetriever.AwaitCallback(true);

// Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
OpenBrowser(authTokenRetriever.AuthURL());

Console.WriteLine("Awaiting Reddit callback. Press any key to abort...");

Console.ReadKey();  // Hit any key to exit.  --Kris

authTokenRetriever.StopListening();

Console.WriteLine("Token retrieval utility terminated.");

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

static void C_AuthSuccess(object? sender, AuthSuccessEventArgs e)
{
  Console.WriteLine("Token retrieval successful!");

  Console.WriteLine();

  Console.WriteLine("Access Token: " + e.AccessToken);
  Console.WriteLine("Refresh Token: " + e.RefreshToken);

  Console.WriteLine();

  Console.WriteLine("Press any key to exit....");
}
