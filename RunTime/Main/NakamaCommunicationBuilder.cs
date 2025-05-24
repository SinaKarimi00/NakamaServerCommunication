using NakamaServerCommunication.RunTime.Application;
using NakamaServerCommunication.RunTime.Main.Services;

namespace NakamaServerCommunication.RunTime.Main
{
    public class NakamaCommunicationBuilder : ICommunicateBuilder
    {
        private readonly NakamaCommunicationSystem _nakamaCommunicationSystem = new NakamaCommunicationSystem();

        public void AddSessionService()
        {
            _nakamaCommunicationSystem.NakamaServices[typeof(SessionService)] = new SessionService();
        }

        public void AddDataService()
        {
            _nakamaCommunicationSystem.NakamaServices[typeof(DataService)] = new DataService();
        }

        public void AddLeaderboardService()
        {
            _nakamaCommunicationSystem.NakamaServices[typeof(LeaderboardService)] = new LeaderboardService();
        }

        public NakamaCommunicationSystem Build()
        {
            return _nakamaCommunicationSystem;
        }
    }
}