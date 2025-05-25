using Nakama;
using Newtonsoft.Json;
using UnityEngine;

namespace NakamaServerCommunication.RunTime.Main.Models
{
    public static class UserPref
    {
        private const string DeviceIDKey = "DeviceIDKey";
        private const string AuthTokenKey = "AuthTokenKey";
        private const string RefreshTokenKey = "RefreshTokenKey";
        private const string DisplayUserNameKey = "DisplayUserNameKey";
        private const string NakamaUserNameKey = "NakamaUserNameKey";
        private const string AvatarIDKey = "AvatarIDKey";
        private const string AvatarFrameIDKey = "AvatarFrameIDKey";

        public static string GetDeviceID()
        {
            var deviceId = PlayerPrefs.GetString(DeviceIDKey, null);

            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;

                if (deviceId == SystemInfo.unsupportedIdentifier)
                {
                    deviceId = System.Guid.NewGuid().ToString();
                }

                PlayerPrefs.SetString(DeviceIDKey, deviceId);
            }

            return deviceId;
        }

        public static string AuthToken => PlayerPrefs.GetString(AuthTokenKey, null);

        public static string RefreshToken => PlayerPrefs.GetString(RefreshTokenKey, null);

        public static string DisplayUsername
        {
            set => PlayerPrefs.SetString(DisplayUserNameKey, value);
            get => PlayerPrefs.GetString(DisplayUserNameKey, null);
        }

        public static string Username
        {
            set => PlayerPrefs.SetString(NakamaUserNameKey, value);
            get => PlayerPrefs.GetString(NakamaUserNameKey, null);
        }

        public static int AvatarID
        {
            set => PlayerPrefs.SetInt(AvatarIDKey, value);
            get => PlayerPrefs.GetInt(AvatarIDKey, 0);
        }

        public static int AvatarFrameID
        {
            set => PlayerPrefs.SetInt(AvatarFrameIDKey, value);
            get => PlayerPrefs.GetInt(AvatarFrameIDKey, 0);
        }

        public static void SaveSessionData(ISession session)
        {
            PlayerPrefs.SetString(AuthTokenKey, session.AuthToken);
            PlayerPrefs.SetString(RefreshTokenKey, session.RefreshToken);
        }

        public static string GetUserMetaData()
        {
            var user = new User()
            {
                Username = DisplayUsername,
                AvatarId = AvatarID.ToString(),
                AvatarFrameId = AvatarFrameID.ToString(),
                DeviceId = GetDeviceID()
            };

            return JsonConvert.SerializeObject(user);
        }
    }
}