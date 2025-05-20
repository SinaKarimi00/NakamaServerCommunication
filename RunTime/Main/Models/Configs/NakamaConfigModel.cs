using UnityEngine;

namespace NakamaServerCommunication.RunTime.Main.Models.Confgis
{
    [CreateAssetMenu(fileName = "NakamaConfig", menuName = "Game/Nakama/NakamaConfig")]
    public class NakamaConfigModel : ScriptableObject
    {
        [SerializeField] private string serverUrl;
        [SerializeField] private string serverKey;
        [SerializeField] private string schema;
        [SerializeField] private int serverPort;
        [SerializeField] private int timeOut;

        public void SetLocalValues()
        {
            serverUrl = "127.0.0.1";
            serverPort = 7350;
            serverKey = "defaultkey";
            schema = "http";
        }

        public void SetStageValues()
        {
            serverUrl = "9486d33a-1cfd-452f-bfd8-828485329992.hsvc.ir";
            serverPort = 31626;
            serverKey = "defaultkey";
            schema = "http";
        }

        public void SetProductValues()
        {
            serverUrl = "09d50bf8-600e-4c30-a50d-2893962cd0bb.hsvc.ir";
            serverPort = 32475;
            serverKey = "defaultkey";
            schema = "http";
        }

        public string ServerUrl => serverUrl;
        public string ServerKey => serverKey;
        public string Schema => schema;
        public int ServerPort => serverPort;
        public int TimeOut => timeOut;
    }
}