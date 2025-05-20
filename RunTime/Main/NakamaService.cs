using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nakama;
using NakamaServerCommunication.RunTime.Main.Models;
using NakamaServerCommunication.RunTime.Main.Models.Confgis;
using NakamaServerCommunication.RunTime.Main.RequestsResponses;
using Newtonsoft.Json;
using UnityEngine;

namespace NakamaServerCommunication.RunTime.Main
{
    public class NakamaService
    {
        public Client Client { get; private set; }
        public ISession Session { get; private set; }

        public void RunNakama(NakamaConfigModel nakamaConfig)
        {
            Client = new Client(nakamaConfig.Schema, nakamaConfig.ServerUrl, nakamaConfig.ServerPort,
                nakamaConfig.ServerKey)
            {
                Timeout = nakamaConfig.TimeOut
            };
        }

        public async Task<ISession> AuthenticationRequest()
        {
            var deviceId = UserPref.GetDeviceID();
            try
            {
                var session = await Client.AuthenticateDeviceAsync(deviceId);
                UserPref.SaveSessionData(session);
                Debug.Log("Authenticated with Device ID");
                return session;
            }
            catch (ApiResponseException ex)
            {
                Debug.LogFormat("Error authenticating with Device ID: {0}", ex.Message);
                return null;
            }
        }

        public async Task<ISession> RefreshSession(ISession session)
        {
            if (session.IsExpired)
            {
                try
                {
                    session = await Client.SessionRefreshAsync(session);
                }
                catch (ApiResponseException)
                {
                    session = await AuthenticationRequest();
                }

                UserPref.SaveSessionData(session);
            }

            return session;
        }

        public async Task<SaveDataResponse> SendSaveDataRequest(IApiWriteStorageObject[] storageObjects)
        {
            try
            {
                await Client.WriteStorageObjectsAsync(Session, storageObjects);
                var response = new SaveDataResponse()
                {
                    IsSuccess = true,
                };
                return response;
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

        public async Task<IApiStorageObjects> SendLoadDataRequest(IApiReadStorageObjectId[] storageObjects)
        {
            var result = await Client.ReadStorageObjectsAsync(Session, storageObjects);
            return result;
        }

        public async Task CreateLeaderboardFromClient(LeaderboardConfig leaderboardConfig)
        {
            var payload = new
            {
                id = leaderboardConfig.Id,
                sort_order = leaderboardConfig.SortOrder,
                leaderboard_operator = leaderboardConfig.LeaderboardOperator,
                reset_schedule = leaderboardConfig.ResetSchedule
            };

            var payloadJson = JsonConvert.SerializeObject(payload);
            var result = await Client.RpcAsync(Session, "create_leaderboard", payloadJson);
            Debug.Log("Leaderboard creation result: " + result.Payload);
        }


        public async Task<List<LeaderboardUser>> FetchLeaderboard(LeaderboardConfig leaderboardConfig)
        {
            var records =
                await Client.ListLeaderboardRecordsAsync(Session, leaderboardConfig.Id,
                    limit: leaderboardConfig.Limit);

            var users = new List<LeaderboardUser>();

            var recordsCopy = records.Records.ToList();


            foreach (var record in recordsCopy)
            {
                users.Add(
                    new LeaderboardUser()
                    {
                        Username = record.Username,
                        Score = record.Score,
                        Rank = record.Rank,
                        MetaData = record.Metadata,
                    }
                );
            }

            return users;
        }


        public async Task SubmitScore(string leaderboardId, int score)
        {
            var user = new User()
            {
                Username = UserPref.DisplayUsername,
                AvatarId = UserPref.AvatarID.ToString(),
                AvatarFrameId = UserPref.AvatarFrameID.ToString(),
                DeviceId = UserPref.GetDeviceID()
            };

            var metadata = JsonConvert.SerializeObject(user);

            await Client.WriteLeaderboardRecordAsync(Session, leaderboardId, score, metadata: metadata);
            Debug.Log("Score submitted!");
        }
    }
}