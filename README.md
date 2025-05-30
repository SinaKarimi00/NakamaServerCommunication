# 🎮 Nakama Server Communication Package for Unity

A modular and extensible Unity package for integrating with [Nakama](https://heroiclabs.com/) game server. It supports
device authentication, session management, profile data, cloud storage, and leaderboard features.
---

## ✨ Features

- 🔐 **Device Authentication** – Simple login using a persistent device ID
- 🔄 **Session Refresh** – Automatically renew expired sessions
- 🧠 **Modular Architecture** – Easily plug services into your own systems
- 💾 **Cloud Save / Load** – Save and retrieve data using Nakama storage
- 🏆 **Leaderboards** – Submit and fetch player rankings
- 🛠️ **Builder Pattern** – Compose only what you need with `NakamaCommunicationBuilder`

---

## 📦 Installation

📥 **Download and import the `.unitypackage` from the [Releases](https://github.com/SinaKarimi00/NakamaServerCommunication/releases) section** of this repository.

---


## 🛠 Server-Side Setup for Leaderboard Creation

Before using the leaderboard features in your Unity package, you must ensure the **Nakama server** has the required server-side RPC function registered.

### Step 1: Create the `create_leaderboard` RPC

Add the following Go code to your **Nakama server** module to enable leaderboard creation via RPC.

```go
package main

import (
    "context"
    "database/sql"
    "encoding/json"

    "github.com/heroiclabs/nakama-common/runtime"
)

type LeaderboardRequest struct {
    ID                  string `json:"id"`
    SortOrder           string `json:"sort_order"`
    LeaderboardOperator string `json:"leaderboard_operator"`
    ResetSchedule       string `json:"reset_schedule"`
}

func CreateLeaderboard(ctx context.Context, logger runtime.Logger, db *sql.DB, nk runtime.NakamaModule, payload string) (string, error) {
    var req LeaderboardRequest
    if err := json.Unmarshal([]byte(payload), &req); err != nil {
        return "", err
    }

    authoritative := false
    metadata := map[string]interface{}{}
    enableRanks := true

    if err := nk.LeaderboardCreate(ctx, req.ID, authoritative, req.SortOrder, req.LeaderboardOperator, req.ResetSchedule, metadata, enableRanks); err != nil {
        logger.Error("Failed to create leaderboard: %v", err)
        return "", err
    }

    logger.Info("Leaderboard '%s' created successfully.", req.ID)
    return `{"success": true}`, nil
}
```

### Step 2: Add  below code to the Initializer or Main Script

```go
    if err := nk.RegisterRpc("create_leaderboard", CreateLeaderboard); err != nil {
        logger.Error("Failed to register RPC 'create_leaderboard': %v", err)
        return nil, err
    }

    logger.Info("RPC 'create_leaderboard' registered successfully.")
```


---
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
  ```

- 🔄 4. Refresh Session (if needed)

  ```csharp
  var newSession = await sessionService.RefreshSession(nakamaSystem.Session, nakamaSystem.Client);
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
    new StorageObjectId
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
  
---

## 🧪 NakamaServerCommunication Unity Test Suite

This project contains runtime integration tests for the [NakamaServerCommunication](https://github.com/SinaKarimi00/NakamaServerCommunication) system using Unity's built-in **Test Runner** and **NUnit**.

---

### 📦 Prerequisites

Before running the tests, ensure the following:

1. **Nakama Server** is running  
   You must run Nakama via Docker or any other method. For Docker

2. **Unity Test Framework** is installed  
   In Unity:
- Open `Window → Package Manager`
- Select `Unity Registry`
- Install **Test Framework**

3. You have the required assets under the `Resources` folder:
- `Resources/NakamaConfig.asset`
- `Resources/LeaderboardConfig.asset`
> ✅ You can load your configs with other ways like    `Addressables`.


---

## 🚀 Running the Tests

To run the test suite in Unity:

1. Open **Unity Editor**
2. Navigate to `Window → General → Test Runner`
3. In the **Test Runner** panel:
- Select **Edit Mode**
- Click **Run All**
4. View results and logs in the **Console**


---

## 🧪 Test List

| Test Method              | Description                                                               |
|--------------------------|---------------------------------------------------------------------------|
| `TestAuthentication`     | Tests user authentication using the configured `NakamaConfig.asset`       |
| `TestRefreshToken`       | Ensures token refresh works after authentication                          |
| `TestSaveData`           | Writes player progress data to storage                                    |
| `TestLoadData`           | Reads player progress data from storage                                   |
| `TestCreateLeaderboard`  | Creates a leaderboard using `LeaderboardConfig.asset`                     |
| `TestSubmitScore`        | Submits a test score to the specified leaderboard                         |
| `TestFetchLeaderboard`   | Fetches and prints leaderboard standings to the console                   |

---

## ⚠️ Known Limitations

- The `Authenticate()` method initializes Nakama and authenticates every time for isolation, but in production tests this can be optimized.
- Ensure the leaderboard ID used (e.g., `"global_leaderboard"`) matches what exists on your Nakama server.

---

