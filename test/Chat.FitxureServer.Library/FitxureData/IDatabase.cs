using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Common.Entities;

namespace Chat.FitxureServer.Library.FitxureData
{
    public interface IDatabase
    {
        public Task SaveChangesAsync();

        public CollectionData<ChatMessage> ChatMessages { get;  }
        public CollectionData<ChatUser> ChatUsers { get;  }

        public Task<ChatUser> CreateUserAsync(string username, string password);
        public Task<ChatUser> LoginAsync(string username, string password);
        public Task<IEnumerable<ChatMessage>> GetChatMessagesAsync();
        public Task<bool> SendMessageAsync(ChatMessage message);
    }
}
