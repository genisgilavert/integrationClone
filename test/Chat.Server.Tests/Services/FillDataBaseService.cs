using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.Services
{
    public class FillDataBaseService
    {
        private readonly IDataSeed _dataSeed;
        public FillDataBaseService(IDataSeed dataSeed)
        {
            _dataSeed = dataSeed;
        }

        public async Task FillChatUsers(HttpClient client)
        {
            var arrayUsers = _dataSeed.GetInitialChatUsers();
            foreach (var item in arrayUsers)
            {
                var postContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", item.Name),
                    new KeyValuePair<string, string>("password", item.Password)
                });
                _ = await client.PostAsync($"api/ChatUsers?username={item.Name}&password={item.Password}", postContent);
            }
        }

        public async Task FillChatMessages(HttpClient client)
        {
            var arrayMessages = _dataSeed.GetInitialChatMessages();
            foreach (var item in arrayMessages)
            {
                _ = await client.PostAsync("api/ChatMessages", new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"));
            }
        }

        public async Task FillAll(HttpClient client){
            await this.FillChatUsers(client);
            await this.FillChatMessages(client);
        }
    }
}
