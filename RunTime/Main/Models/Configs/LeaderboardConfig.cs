using UnityEngine;

namespace NakamaServerCommunication.RunTime.Main.Models.Confgis
{
    [CreateAssetMenu(fileName = "LeaderboardConfig", menuName = "Game/Leaderboard/Leaderboard Config")]
    public class LeaderboardConfig : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string sortOrder;
        [SerializeField] private string leaderboardOperator;
        [SerializeField] private string resetSchedule;
        [SerializeField] private int limit;

        public string Id => id;
        public string SortOrder => sortOrder;
        public string LeaderboardOperator => leaderboardOperator;
        public string ResetSchedule => resetSchedule;
        public int Limit => limit;
    }
}