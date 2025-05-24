using Nakama;
using UnityEngine;
using System.Threading.Tasks;
using NakamaServerCommunication.RunTime.Application;
using NakamaServerCommunication.RunTime.Main.Models;

namespace NakamaServerCommunication.RunTime.Main.Services
{
    public class SessionService : INakamaService
    {
        public async Task<ISession> AuthenticationRequest(IClient client)
        {
            var deviceId = UserPref.GetDeviceID();
            try
            {
                var session = await client.AuthenticateDeviceAsync(deviceId);
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

        public async Task<ISession> RefreshSession(ISession session, IClient client)
        {
            if (session.IsExpired)
            {
                try
                {
                    session = await client.SessionRefreshAsync(session);
                }
                catch (ApiResponseException)
                {
                    session = await AuthenticationRequest(client);
                }

                UserPref.SaveSessionData(session);
            }

            return session;
        }
    }
}