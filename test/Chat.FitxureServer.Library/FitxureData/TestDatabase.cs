using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Common.Entities;

namespace Chat.FitxureServer.Library.FitxureData
{
    public class TestDatabase : IDatabase
    {
        private CollectionData<ChatMessage> _chatMessages;
        private CollectionData<ChatUser> _chatUsers;
        private static int _idUser;
        private static int _idMessage;
        private static IDatabase _instance = null;
        private TestDatabase()
        {
            this.InitListUsers();
            this.InitListMessages();
            _idUser = 11;
            _idMessage = 11;
        }

        public static IDatabase Instance(){
            if(_instance == null){
                _instance = new TestDatabase();
            }
            return _instance;
        }

        public CollectionData<ChatMessage> ChatMessages { get => _chatMessages; }
        public CollectionData<ChatUser> ChatUsers { get => _chatUsers; }

        public async Task SaveChangesAsync()
        {
            await Task.Delay(1);
        }
        private void InitListUsers()
        {
            ChatUser[] chatUsers = new ChatUser[]{
                this.CreateUser(1,"Usuario1","P2ssw0rd!"),
                this.CreateUser(2,"Usuario2","P2ssw0rd!"),
                this.CreateUser(3,"Usuario3","P2ssw0rd!")
            };
            _chatUsers = new CollectionData<ChatUser>(chatUsers);
        }

        private void InitListMessages() {
            var date = DateTime.Now;
            ChatMessage[] chatMessages = new ChatMessage[] { 
                this.CreateMessage(3,"Usuario3","Test",date.AddMinutes(-10)),
                this.CreateMessage(4,"Author1", "Hello everybody!",date),
                this.CreateMessage(5,"Author2","Hello Author1",date.AddMinutes(1)),
                this.CreateMessage(6,"Author1","How are you?",date.AddMinutes(2))
            };
            _chatMessages = new CollectionData<ChatMessage>(chatMessages);
        }

        private ChatUser CreateUser(int idUser, string name, string password){
            return new ChatUser() {
                IdUser = idUser,
                Name = name,
                Password = password
            };
        }

        private ChatMessage CreateMessage(int id, string author, string message, DateTime date)
        {
            return new ChatMessage() {
                IdChatMessage = id,
                Author = author,
                Message = message,
                Date = date
            };
        }

        public async Task<ChatUser> CreateUserAsync(string username, string password)
        {
            ChatUser chatUser = this.CreateUser(_idUser++,username, password);
            await Task.Delay(1);
            this.ChatUsers.Add(chatUser);
            return chatUser;

        }

        public async Task<ChatUser> LoginAsync(string username, string password)
            => await this.ChatUsers.FirstAsync(p => p.Name == username && p.Password == password);

        public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(){
            await Task.Delay(1);
            return this.ChatMessages;
        }

        public async Task<bool> SendMessageAsync(ChatMessage message)
        {
            message.IdChatMessage = _idMessage++;
            await Task.Delay(1);
            this.ChatMessages.Add(message);
            return true;
        }
    }
}
