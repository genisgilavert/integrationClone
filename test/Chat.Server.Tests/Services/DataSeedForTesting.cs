using Chat.Common.Entities;
using Chat.FitxureServer.Library.FitxureData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Tests.Services
{
    public class DataSeedForTesting : IDataSeed
    {
        private IDatabase _testDatabase;
        
        public DataSeedForTesting(IDatabase database){
            _testDatabase = database;
        }

        public ChatUser[] GetInitialChatUsers(){
            return _testDatabase.ChatUsers.ToArray();
        }
        public ChatMessage[] GetInitialChatMessages(){
            return _testDatabase.ChatMessages.ToArray();
        }
    }
}
