using System;

namespace NakamaServerCommunication.RunTime.Main.Models
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string DeviceId { get; set; }
        public string AvatarId { get; set; }
        public string AvatarFrameId { get; set; }
    }
}