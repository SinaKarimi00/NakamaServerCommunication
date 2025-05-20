using System;
using UnityEngine;

namespace NakamaServerCommunication.RunTime.Main.Models
{
    [Serializable]
    public class LeaderboardUser
    {
        [SerializeField] private string rank;

        [SerializeField] private string score;

        [SerializeField] private string username;
        private string _metaData;

        public string Rank
        {
            get => rank;
            set => rank = value;
        }

        public string Score
        {
            get => score;
            set => score = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public string MetaData
        {
            get => _metaData;
            set => _metaData = value;
        }
    }
}