using System;
using Nakama;
using System.Threading.Tasks;
using NakamaServerCommunication.RunTime.Application;
using NakamaServerCommunication.RunTime.Main.RequestsResponses;

namespace NakamaServerCommunication.RunTime.Main.Services
{
    public class DataService : INakamaService
    {
        public async Task<SaveDataResponse> SaveData(ISession session, IClient client,
            IApiWriteStorageObject[] storageObjects)
        {
            try
            {
                await client.WriteStorageObjectsAsync(session, storageObjects);
                var response = new SaveDataResponse()
                {
                    IsSuccess = true,
                };
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IApiStorageObjects> LoadData(ISession session, IClient client,
            IApiReadStorageObjectId[] storageObjects)
        {
            var result = await client.ReadStorageObjectsAsync(session, storageObjects);
            return result;
        }
    }
}