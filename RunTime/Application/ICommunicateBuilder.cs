using NakamaServerCommunication.RunTime.Main.Services;

namespace NakamaServerCommunication.RunTime.Application
{
    public interface ICommunicateBuilder
    {
        public void AddSessionService();
        public void AddDataService();
        public void AddLeaderboardService();
    }
}