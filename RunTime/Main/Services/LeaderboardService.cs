using Nakama;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using NakamaServerCommunication.RunTime.Application;
using NakamaServerCommunication.RunTime.Main.Models;
using NakamaServerCommunication.RunTime.Main.Models.Confgis;


namespace NakamaServerCommunication.RunTime.Main.Services
{
    public class LeaderboardService : INakamaService
    {
        public async Task CreateLeaderboardFromClient(ISession session, IClient client,
            LeaderboardConfig leaderboardConfig)
        {
            var payload = new
            {
                id = leaderboardConfig.Id,
                sort_order = leaderboardConfig.SortOrder,
                leaderboard_operator = leaderboardConfig.LeaderboardOperator,
                reset_schedule = leaderboardConfig.ResetSchedule
            };

            var payloadJson = JsonConvert.SerializeObject(payload);
            var result = await client.RpcAsync(session, "create_leaderboard", payloadJson);
            Debug.Log("Leaderboard creation result: " + result.Payload);
        }


        public async Task<List<LeaderboardUser>> FetchLeaderboard(ISession session, IClient client,
            LeaderboardConfig leaderboardConfig)
        {
            var records =
                await client.ListLeaderboardRecordsAsync(session, leaderboardConfig.Id,
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


        public async Task SubmitScore(ISession session, IClient client, string leaderboardId, int score)
        {
            var user = new User()
            {
                Username = UserPref.DisplayUsername,
                AvatarId = UserPref.AvatarID.ToString(),
                AvatarFrameId = UserPref.AvatarFrameID.ToString(),
                DeviceId = UserPref.GetDeviceID()
            };

            var metadata = JsonConvert.SerializeObject(user);

            await client.WriteLeaderboardRecordAsync(session, leaderboardId, score, metadata: metadata);
            Debug.Log("Score submitted!");
        }
    }
}