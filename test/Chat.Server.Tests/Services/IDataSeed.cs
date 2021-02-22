using Chat.Common.Entities;
using Chat.FitxureServer.Library.FitxureData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.Services
{
    public interface IDataSeed
    {
        public ChatUser[] GetInitialChatUsers();
        public ChatMessage[] GetInitialChatMessages();
    }
}
