using System;
using System.Linq;
using System.Threading.Tasks;
using Nakama;
using NakamaServerCommunication.RunTime.Main;
using NakamaServerCommunication.RunTime.Main.Models.Confgis;
using NakamaServerCommunication.RunTime.Main.Services;
using NUnit.Framework;
using UnityEngine;

namespace NakamaServerCommunication.Tests.RunTime
{
    public class NakamaServerCommunicationSample
    {
        private NakamaCommunicationSystem _nakamaCommunicationSystem;
        private IClient _client;
        private ISession _session;

        [Test]
        public async void Start()
        {
            var nakamaServerCommunicationBuilder = new NakamaCommunicationBuilder();
            nakamaServerCommunicationBuilder.AddSessionService();
            nakamaServerCommunicationBuilder.AddDataService();
            nakamaServerCommunicationBuilder.AddLeaderboardService();
            _nakamaCommunicationSystem = nakamaServerCommunicationBuilder.Build();

            var nakamaConfig = Resources.Load<NakamaConfigModel>("NakamaConfig");
            _client = _nakamaCommunicationSystem.RunNakama(nakamaConfig);
        }

        [Test]
        public async void TestAuthentication()
        {
            await Authenticate();
        }

        [Test]
        public async void TestRefreshToken()
        {
            try
            {
                await Authenticate(); //TODO in runtime we should not authenticate for each request, I use this just for test
                _session = await _nakamaCommunicationSystem.GetService<SessionService>(typeof(SessionService))
                    .RefreshSession(_session, _client);

                Debug.Log("RefreshToken Success: " + _session.AuthToken);
            }
            catch (Exception e)
            {
                Debug.Log("RefreshToken Failed: " + e);
            }
        }

        [Test]
        public async void TestSaveData()
        {
            try
            {
                await Authenticate(); //TODO in runtime we should not authenticate for each request, I use this just for test
                var storageObjects = new IApiWriteStorageObject[]
                {
                    new WriteStorageObject
                    {
                        Collection = "player_data",
                        Key = "progress",
                        Value = "{\"level\":5}"
                    }
                };

                var result = await _nakamaCommunicationSystem.GetService<DataService>(typeof(DataService))
                    .SaveData(_session, _client, storageObjects);

                Debug.Log(result.IsSuccess ? "SaveData Success" : "SaveData Failed");
            }
            catch (Exception e)
            {
                Debug.Log("SaveData Failed, " + e);
            }
        }

        [Test]
        public async void TestLoadData()
        {
            try
            {
                await Authenticate(); //TODO in runtime we should not authenticate for each request, I use this just for test
                var readObjects = new IApiReadStorageObjectId[]
                {
                    new StorageObjectId
                    {
                        Collection = "player_data",
                        Key = "progress",
                        UserId = _session.UserId
                    }
                };

                var result = await _nakamaCommunicationSystem.GetService<DataService>(typeof(DataService))
                    .LoadData(_session, _client, readObjects);

                Debug.Log("LoadData Success: " + result.Objects.First().ToString());
            }
            catch (Exception e)
            {
                Debug.Log("LoadData Failed: " + e);
            }
        }

        [Test]
        public async void TestCreateLeaderboard()
        {
            try
            {
                await Authenticate(); //TODO in runtime we should not authenticate for each request, I use this just for test
                var leaderboardService =
                    _nakamaCommunicationSystem.GetService<LeaderboardService>(typeof(LeaderboardService));
                var leaderboardConfig = Resources.Load<LeaderboardConfig>("LeaderboardConfig");

                await leaderboardService.CreateLeaderboardFromClient(_session, _client, leaderboardConfig);
            }
            catch (Exception e)
            {
                Debug.Log("CreateLeaderboard Failed: " + e);
            }
        }

        [Test]
        public async void TestSubmitScore()
        {
            try
            {
                await Authenticate(); //TODO in runtime we should not authenticate for each request, I use this just for test
                var leaderboardService =
                    _nakamaCommunicationSystem.GetService<LeaderboardService>(typeof(LeaderboardService));
                await leaderboardService.SubmitScore(_session, _client, "global_leaderboard", 10000);
            }
            catch (Exception e)
            {
                Debug.Log("SubmitScore Failed: " + e);
            }
        }

        [Test]
        public async void TestFetchLeaderboard()
        {
            try
            {
                await Authenticate(); //TODO in runtime we should not authenticate for each request, I use this just for test
                var leaderboardService =
                    _nakamaCommunicationSystem.GetService<LeaderboardService>(typeof(LeaderboardService));
                var leaderboardConfig = Resources.Load<LeaderboardConfig>("LeaderboardConfig");

                var entries = await leaderboardService.FetchLeaderboard(_session, _client, leaderboardConfig);
                foreach (var player in entries)
                {
                    Debug.Log($"{player.Rank}. {player.Username} - {player.Score}");
                }
            }
            catch (Exception e)
            {
                Debug.Log("FetchLeaderboard Failed: " + e);
            }
        }


        private async Task Authenticate()
        {
            Debug.Log("Testing Authentication");
            try
            {
                Start();
                _session = await _nakamaCommunicationSystem.GetService<SessionService>(typeof(SessionService))
                    .AuthenticationRequest(_client);

                Debug.Log("Authentication Success: " + _session.AuthToken);
            }
            catch (Exception e)
            {
                Debug.Log("Authentication Failed: " + e);
            }
        }
    }
}