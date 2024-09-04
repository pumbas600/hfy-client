using System.Diagnostics;
using System.Runtime.InteropServices;
using HfyClientApi.Configuration;
using Microsoft.Extensions.Configuration;
using Reddit.AuthTokenRetriever;
using Reddit.AuthTokenRetriever.EventArgs;

/*
 * This has been modified from:
  https://github.com/sirkris/Reddit.NET/blob/master/src/AuthTokenRetriever/Program.cs
 */


var configuration = new ConfigurationBuilder()
  .AddUserSecrets<Program>()
  .Build();

var authTokenRetriever = new AuthTokenRetrieverLib(
  configuration[Config.Keys.RedditAppId],
  8080,
  appSecret: configuration[Config.Keys.RedditAppSecret]

);

authTokenRetriever.AuthSuccess += C_AuthSuccess;
authTokenRetriever.AwaitCallback(true);

Console.WriteLine();
Console.WriteLine("** IMPORTANT:  Before you proceed any further, make sure you are logged into Reddit as the user you wish to authenticate! **");
Console.WriteLine();

Console.WriteLine("In the next step, a browser window will open and you'll be taken to Reddit's app authentication page.  Press any key to continue....");
Console.ReadKey();

// Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
OpenBrowser(authTokenRetriever.AuthURL());

Console.WriteLine("Awaiting Reddit callback. Press any key to abort...");

Console.ReadKey();  // Hit any key to exit.  --Kris

authTokenRetriever.StopListening();

Console.WriteLine("Token retrieval utility terminated.");

static void OpenBrowser(string authUrl = "about:blank")
{
  if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
  {
    try
    {
      ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
      Process.Start(processStartInfo);
    }
    catch (System.ComponentModel.Win32Exception)
    {
      Console.WriteLine("Failed to open browser. Please manually navigate to the following URL:");
      Console.WriteLine(authUrl);
    }
  }
  else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
  {
    // For OSX run a separate command to open the web browser as found in https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
    Process.Start("open", authUrl);
  }
  else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
  {
    // Similar to OSX, Linux can (and usually does) use xdg for this task.
    Process.Start("xdg-open", authUrl);
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
