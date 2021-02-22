using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Chat.Server.Controllers;
using Chat.FitxureServer.Library.FitxureData;
using Moq;
using Chat.Server.Library.Data;
using Microsoft.EntityFrameworkCore;
using Chat.Common.Entities;
using Moq.Protected;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Chat.Server.Tests.Services;

namespace Chat.Server.Tests
{
    public class ChatMessagesControllerTests
    {
        private readonly TestServer _server;
        private readonly FillDataBaseService _fillDataBaseService;
        private readonly IDatabase _database;
        public ChatMessagesControllerTests()
        {
            var builder = new WebHostBuilder()
                    .UseStartup<TestStartup>();
            _server = new TestServer(builder);

            _database = TestDatabase.Instance();
            _fillDataBaseService = new FillDataBaseService(new DataSeedForTesting(_database));
        }

        [Fact]
        public async Task GetChatMessages_shouldReturnChatMessagesAsync(){

            //Arrange
            var client = _server.CreateClient();
            var list = _database.ChatMessages.ToList();
            await this._fillDataBaseService.FillAll(client);
            //Act
            var response = await client.GetAsync($"api/ChatMessages");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ChatMessage>>(content);
            //Assert
            result.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task PostChatMessage_shouldReturnChatMessageAsync(){

            //Arrange
            var client = _server.CreateClient();
            await this._fillDataBaseService.FillAll(client);

            var chatMessage = new ChatMessage() {
                IdChatMessage = -1,
                Author = "Pepe Lospalotes",
                Date = DateTime.Now,
                Message = "test message"
            };
            //Act
            var response = await client.PostAsync("api/ChatMessages", new StringContent(JsonConvert.SerializeObject(chatMessage), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ChatMessage>(content);
            //Assert
            result.Author.Should().BeEquivalentTo(chatMessage.Author);
            result.Date.Should().BeSameDateAs(chatMessage.Date);
            result.Message.Should().BeEquivalentTo(chatMessage.Message);
        }
    }
}
