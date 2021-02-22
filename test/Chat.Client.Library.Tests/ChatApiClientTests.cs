using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Chat.Client.Library.Services;
using System.Net.Http;
using Chat.FitxureServer.Library.Services;
using Chat.Common.Entities;
using Chat.FitxureServer.Library.FitxureData;

namespace Chat.Client.Library.Tests
{
    public class ChatApiClientTests
    {
        private readonly ChatApiClient _chatApiClient;   
        public ChatApiClientTests(){
            var httpClient = new HttpClient(new FakeMessageHandler()) {
                BaseAddress = new Uri("http://localhost")
            };
            _chatApiClient = new ChatApiClient(httpClient);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldBeTrue(){

            //Arrange
            var message = new ChatMessage() { Author = string.Empty, Date = DateTime.Now, IdChatMessage = -1 , Message = "test message"};
            //Act
            var result = await _chatApiClient.SendMessageAsync(message);
            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task GetChatMessageASync_ShouldBeOfTypeExpectedResult(){ 

            //Arrange
            var listChatMessages = TestDatabase.Instance().ChatMessages.ToList();
            //Act
            var result = await _chatApiClient.GetChatMessagesAsync();
            //Assert
            result.Should().BeEquivalentTo(listChatMessages);
        }
    }
}
