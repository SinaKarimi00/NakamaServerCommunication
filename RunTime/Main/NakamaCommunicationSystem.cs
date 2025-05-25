using Nakama;
using System;
using System.Collections.Generic;
using NakamaServerCommunication.RunTime.Application;
using NakamaServerCommunication.RunTime.Main.Models.Confgis;

namespace NakamaServerCommunication.RunTime.Main
{
    public class NakamaCommunicationSystem
    {
        public readonly Dictionary<Type, INakamaService> NakamaServices = new Dictionary<Type, INakamaService>();

        public Client RunNakama(NakamaConfigModel nakamaConfig)
        {
            var client = new Client(nakamaConfig.Schema, nakamaConfig.ServerUrl, nakamaConfig.ServerPort,
                nakamaConfig.ServerKey)
            {
                Timeout = nakamaConfig.TimeOut
            };
            return client;
        }

        public T GetService<T>(Type serviceType) where T : class, INakamaService
        {
            if (NakamaServices.TryGetValue(serviceType, out var service))
            {
                return service as T;
            }

            return null;
        }
    }
}