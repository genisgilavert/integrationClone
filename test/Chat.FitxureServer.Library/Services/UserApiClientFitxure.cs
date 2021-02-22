using Chat.Client.Library.Services;
using Chat.Common.Entities;
using Chat.FitxureServer.Library.FitxureData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.FitxureServer.Library.Services
{
    public class UserApiClientFitxure : IUserApiClient
    {
        private readonly IDatabase _dbContext;
        public UserApiClientFitxure(){
            _dbContext = TestDatabase.Instance();
        }
        public UserApiClientFitxure(IDatabase database){
            _dbContext = database;
        }
        public Task<ChatUser> CreateUserAsync(string username, string password)
            => this._dbContext.CreateUserAsync(username,password);

        public Task<ChatUser> LoginAsync(string username, string password)
            => this._dbContext.LoginAsync(username,password);
    }
}
