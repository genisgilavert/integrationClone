using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using Chat.Server.Library.Data;
using Chat.Server.Controllers;
using Chat.FitxureServer.Library.FitxureData;
using Chat.Common.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Chat.Server.Tests.Services;

namespace Chat.Server.Tests
{
    public class ChatUsersControllerTests
    {
        private readonly TestServer _server;
        private readonly FillDataBaseService _fillDataBaseService;
        private readonly IDatabase _database;
        public ChatUsersControllerTests()
        {
            var builder = new WebHostBuilder()
                    .UseStartup<TestStartup>();
            _server = new TestServer(builder);

            _database = TestDatabase.Instance();
            _fillDataBaseService = new FillDataBaseService(new DataSeedForTesting(_database));
        }

        [Fact]
        public async Task GetChatUser_shouldReturnResultExpected(){
            //Arrange
            var client = _server.CreateClient();
            var user = _database.ChatUsers.First();
            await this._fillDataBaseService.FillAll(client);
            //Act
            var response = await client.GetAsync($"api/ChatUsers?username={user.Name}&password={user.Password}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ChatUser>(content);
            //Assert
            result.Name.Should().BeEquivalentTo(user.Name);
            result.Password.Should().BeEquivalentTo(user.Password);
        }

        [Fact]
        public async Task GetChatUser_shouldReturnDefaultUserIfUserNotExist(){
            //Arrange
            var client = _server.CreateClient();
            await this._fillDataBaseService.FillAll(client);

            ChatUser user = new ChatUser() { 
                IdUser = -1,
                Name = "",
                Password = ""
            };
            //Act
            var response = await client.GetAsync($"api/ChatUsers?username={user.Name}&password={user.Password}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ChatUser>(content);
            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task PostChatUser_shouldReturnNewUser(){
            //Arrange
            var client = _server.CreateClient();
            await this._fillDataBaseService.FillAll(client);
            
            ChatUser user = new ChatUser()
            {
                IdUser = -1,
                Name = "newName",
                Password = "newPassword"
            };
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", user.Name),
                new KeyValuePair<string, string>("password", user.Password)
            });
            //Act
            var response = await client.PostAsync($"api/ChatUsers?username={user.Name}&password={user.Password}", postContent);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ChatUser>(content);
            //Assert
            result.Name.Should().BeEquivalentTo(user.Name);
            result.Password.Should().BeEquivalentTo(user.Password);
        }
    }
}
