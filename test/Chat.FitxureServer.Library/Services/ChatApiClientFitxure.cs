using Chat.Client.Library.Services;
using Chat.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.FitxureServer.Library.FitxureData;

namespace Chat.FitxureServer.Library.Services
{
    public class ChatApiClientFitxure : IChatApiClient
    {
        private readonly IDatabase _dbContext;

        public ChatApiClientFitxure()
        {
            _dbContext = TestDatabase.Instance();
        }
        public ChatApiClientFitxure(IDatabase database){
            _dbContext = database;
        }
        public Task<IEnumerable<ChatMessage>> GetChatMessagesAsync()
            => this._dbContext.GetChatMessagesAsync();

        public Task<bool> SendMessageAsync(ChatMessage message)
            => this._dbContext.SendMessageAsync(message);
    }
}
