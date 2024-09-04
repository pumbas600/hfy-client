## ✅ Prerequisites

- [.NET 8.0+ SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) — Download the installer for your OS and follow the installation instructions.
- [Reddit Application](https://www.reddit.com/prefs/apps) — Click "create another app..." to get your app id and secret.
- PostgreSQL Database — You can use a local or remote database.

## ⚙️ Setup

Before running the backend, you need to setup the necessary environment variables in the [`/HfyClientApi`](./HfyClientApi/) directory:

```bash
dotnet user-secrets set "RedditAppId" "YOUR_APP_ID"

dotnet user-secrets set "RedditAppSecret" "YOUR_APP_SECRET"

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=5432;Database=hfy-client;Username=...;Password=..."
```

After setting the above credentials, you'll also need a refresh token to authenticate with the Reddit API. This can be obtained using the [`/HfyClientConsole`](./HfyClientConsole/) project:

```bash
dotnet run
```

> [!IMPORTANT]
> You'll need to first set your [Reddit application's](https://www.reddit.com/prefs/apps) redirect URI to `http://127.0.0.1:8080/Reddit.NET/oauthRedirect`.

You can then set the `RedditRefreshToken` user secret:

```bash
dotnet user-secrets set "RedditRefreshToken" "YOUR_REFRESH_TOKEN"
```

### Database

To update your database with the correct schema, run the following commands in the [`/HfyClientApi`](./HfyClientApi/) directory:

```bash
dotnet ef database update
```

## 🚀 Run

To start the API, run the following command in the [`/HfyClientApi`](./HfyClientApi/) directory:

```bash
dotnet run

# or, for live-updating:

dotnet watch
```

### Updating the Database

If you make changes to the database schema, you can generate a migration using:

```bash
dotnet ef migrations add "Migration-Name"
```

To update the database with the new migration, run:

```bash
dotnet ef database update
```
