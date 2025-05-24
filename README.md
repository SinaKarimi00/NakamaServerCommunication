# 🎮 Nakama Server Communication Package for Unity

A modular and extensible Unity package for integrating with [Nakama](https://heroiclabs.com/) game server. It supports
device authentication, session management, profile data, cloud storage, and leaderboard features

---

## ✨ Features

- 🔐 **Device Authentication** – Simple login using a persistent device ID
- 🔄 **Session Refresh** – Automatically renew expired sessions
- 🧠 **Modular Architecture** – Easily plug services into your own systems
- 💾 **Cloud Save / Load** – Save and retrieve data using Nakama storage
- 🏆 **Leaderboards** – Submit and fetch player rankings
- 🛠️ **Builder Pattern** – Compose only what you need with `NakamaCommunicationBuilder`

---

## 📦 Dependencies

This package depends on the following libraries(these dependencies are imported in this package, you can skip this section):

### ✅ [Nakama Unity SDK](https://github.com/heroiclabs/nakama-unity)

Used for communication with Nakama game server.

Add to your `manifest.json`:

```json
"com.heroiclabs.nakama-unity": "https://github.com/heroiclabs/nakama-unity.git#3.15.0"
```

This package depends on the following libraries:

### ✅ [Newtonsoft.Json](https://github.com/applejag/Newtonsoft.Json-for-Unity)

Used for JSON serialization (e.g. leaderboard metadata).

Add to your `manifest.json`:

```json
"com.unity.nuget.newtonsoft-json": "3.0.2"
```

## 🚀 Usage

- 🔧 1. Initialize the Builder, add needed services to builder

  ```csharp
  var builder = new NakamaCommunicationBuilder();
  builder.AddSessionService();
  builder.AddDataService();
  builder.AddLeaderboardService();
  var nakamaSystem = builder.Build();
    ```

- 🌐 2. Start the Client, in this section, you should pass your server config to the RunNakama method, you can create
  a server config with this menu name ```Game/Nakama/NakamaConfig``` in the project folder and load it by resources or
  addressable or another way that you want.
  ```csharp
  _nakamaConfig = Addressables.LoadAssetAsync<NakamaConfig>("NakamaConfig").WaitForCompletion();
  nakamaSystem.RunNakama(_nakamaConfig);
  ```

- 🔐 3. Authenticate with Device ID

  ```csharp
  var sessionService = nakamaSystem.GetService<SessionService>(typeof(SessionService));
  var session = await sessionService.AuthenticationRequest(nakamaSystem.Client);
  nakamaSystem.Session = session;
  ```

- 🔄 4. Refresh Session (if needed)

  ```csharp
  nakamaSystem.Session = await sessionService.RefreshSession(nakamaSystem.Session, nakamaSystem.Client);
  ```

- 💾 5. Save Data
  ```csharp
  var dataService = nakamaSystem.GetService<DataService>(typeof(DataService));
  var storageObjects = new IApiWriteStorageObject[]
  {
    new WriteStorageObject
    {
      Collection = "player_data",
      Key = "progress",
      Value = "{\"level\":5}"
    }
  };
            
  await dataService.SaveData(nakamaSystem.Session, nakamaSystem.Client, storageObjects);
  ```
- 📥 6. Load Data
  ```csharp
  var readObjects = new IApiReadStorageObjectId[]
  {
    new ApiReadStorageObjectId
    {
      Collection = "player_data",
      Key = "progress",
      UserId = nakamaSystem.Session.UserId
    }
  };
  var result = await dataService.LoadData(nakamaSystem.Session, nakamaSystem.Client, readObjects);
  ```

- 🆕 9. Create Leaderboard (RPC Call), you should pass your leaderboard config to the
  CreateLeaderboardFromClient method, you can create
  a leaderboard config with this menu name ```Game/Leaderboard/Leaderboard Config``` in the project folder
  and load it by resources or
  addressable or another way that you want.

  ```csharp
  var leaderboardService = nakamaSystem.GetService<LeaderboardService>(typeof(LeaderboardService));
  var config = new LeaderboardConfig
  {
      Id = "global_leaderboard",
      SortOrder = 0, // 0: Descending, 1: Ascending
      LeaderboardOperator = 0, // 0: Best, 1: Set, 2: Increment
      ResetSchedule = "0 0 * * 1" // Weekly reset (CRON format)
  };
      
  await leaderboardService.CreateLeaderboardFromClient(nakamaSystem.Session, nakamaSystem.Client, config);

  ```

- 🏆 8. Submit Leaderboard Score
  ```csharp
  var leaderboardService = nakamaSystem.GetService<LeaderboardService>(typeof(LeaderboardService));
  await leaderboardService.SubmitScore(nakamaSystem.Session, nakamaSystem.Client, "global_leaderboard", 10000);
  ```

- 📊 9. Fetch Leaderboard
  ```csharp
  var config = new LeaderboardConfig
  {
    Id = "global_leaderboard",
    Limit = 10
  };
          
  var entries = await leaderboardService.FetchLeaderboard(nakamaSystem.Session, nakamaSystem.Client, config);
  foreach (var player in entries)
  {
    Debug.Log($"{player.Rank}. {player.Username} - {player.Score}");
  }
  ```