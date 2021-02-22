using Chat.Client.Library.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Entities;
using Xunit;
using Chat.FitxureServer;
using Chat.FitxureServer.Library.Services;
using Chat.FitxureServer.Library.FitxureData;
using Moq;
using FluentAssertions;

namespace Chat.Client.Library.Tests
{
    public class ChatClientTests : IDisposable, IAsyncLifetime
    {
        private readonly IChatClient _chatClient;

        public ChatClientTests()
        {
            // lists
            var listUsers = TestDatabase.Instance().ChatUsers;
            var listChatMessages = TestDatabase.Instance().ChatMessages;

            // vars
            var chatUser = new ChatUser();
            var chatMessage = new ChatMessage();

            // moq setting IDataBase
            var database = Mock.Of<IDatabase>(MockBehavior.Default);
            Mock.Get(database).SetupGet(db => db.ChatUsers).Returns(listUsers);
            Mock.Get(database).SetupGet(db => db.ChatMessages).Returns(listChatMessages);

            Mock.Get(database).Setup(db => db.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string username, string password) => chatUser = CreateChatUser(username, password));

            Mock.Get(database).Setup(db => db.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(chatUser = null);

            foreach (ChatUser item in listUsers.GetEnumerator()){
                Mock.Get(database).Setup(db => db.LoginAsync(item.Name, item.Password))
                /*
                .Callback((string username, string password) => { 
                    chatUser = this.CreateChatUser(username, password); 
                    })
                */
                .ReturnsAsync((string username, string password) => _ = this.CreateChatUser(username, password));
            }

            Mock.Get(database).Setup(db => db.GetChatMessagesAsync()).ReturnsAsync(listChatMessages);

            //Mock.Get(database).Setup(db => db.SendMessageAsync(It.IsAny<ChatMessage>())).Returns(Task.FromResult(true)); es lo mismo que la línea de abajo
            Mock.Get(database).Setup(db => db.SendMessageAsync(It.IsAny<ChatMessage>())).ReturnsAsync(true);

            // Init ChatApiClientFitxure and UserApiClientFitxure 
            var userApiClient = new UserApiClientFitxure(database);
            var chatApiClient = new ChatApiClientFitxure(database);

            //creacion de IchatClient
            _chatClient = new ChatClient(chatApiClient, userApiClient);
        }

        private ChatUser CreateChatUser(string username, string password, int id = -1)
        {
            return new ChatUser()
            {
                IdUser = id,
                Name = username,
                Password = password
            };
        }

        [Theory]
        [InlineData(true, "Usuario1", "P2ssw0rd!")]
        [InlineData(false, "Usuario2", "")]
        [InlineData(false, "", "P2ssw0rd!")]
        [InlineData(false, "UserNotExist", "P2ssw0rd!")]
        public async Task LoginAsync_ShouldBeExpectedResult(bool expected, string username, string password)
        {
            //Arrange

            //Act
            var result = await _chatClient.LoginAsync(username, password);

            //Assert
            //Assert.Equal(expected, result);
            result.Should().Be(expected);
        }

        [Fact]
        public void LoginAsync_ShouldBeArgumentNullException_IfArgumentNull()
        {
            //Arrange

            //Act
            Func<Task> act = async () => { _ = await _chatClient.LoginAsync(null, null); };

            //Assert
            //Assert.ThrowsAsync<ArgumentNullException>(act);
            act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(true, "Usuario1", "P2ssw0rd!")]
        [InlineData(false, "Usuario2", "")]
        [InlineData(false, "", "P2ssw0rd!")]
        public async Task CreateUserAsync_ShouldBeExpectedResult(bool expected, string username, string password)
        {
            //Arrange

            //Act
            var result = await _chatClient.CreateUserAsync(username, password);

            //Assert
            //Assert.Equal(expected, result);
            result.Should().Be(expected);
        }

        [Fact]
        public void CreateUserAsync_ShouldBeArgumentNullException_IfArgumentNull()
        {
            //Arrange

            //Act
            Func<Task> act = async () => { _ = await _chatClient.CreateUserAsync(null, null); };

            //Assert
            //Assert.ThrowsAsync<ArgumentNullException>(act);
            act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SendMessageAsync_ShouldBeTrue_IfLoggedAndConnected()
        {
            //Arrange
            var message = "Message1";
            await _chatClient.LoginAsync("Usuario1", "P2ssw0rd!");
            _chatClient.Connect();

            //Act
            var result = await _chatClient.SendMessageAsync(message);

            //Assert
            //Assert.True(result);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task SendMessageAsync_ShouldBeFalse_IfNotLogged(){

            //Arrange
            var message = "Message1";
            _chatClient.Connect();
            //Act
            var result = await _chatClient.SendMessageAsync(message);
            //Assert
            result.Should().BeFalse();
        }
        [Fact]
        public async Task SendMessageAsync_ShouldBeFalse_IfNotConnected(){

            //Arrange
            var message = "Message1";
            var user = await TestDatabase.Instance().ChatUsers.FirstAsync(u => u.Name == "Usuario3");
            await _chatClient.LoginAsync(user.Name, user.Password);
            //Act
            var result = await _chatClient.SendMessageAsync(message);
            //Assert
            result.Should().BeFalse();
        }
        [Fact]
        public async Task SendMessageAsync_ShouldBeFalse_IfNotLoggedAndNotConnected(){

            //Arrange
            var message = "Message1";
            //Act
            var result = await _chatClient.SendMessageAsync(message);
            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task NewMessageRecived_ShouldBeRaised_IfConnect()
        {
            using (var monitoredSubject = _chatClient.Monitor()){
                //Arrange
                //var eventRecived = false;
                await _chatClient.LoginAsync("Usuario1", "P2ssw0rd!");
                //_chatClient.NewMessageRecived += (sender, _) => eventRecived = true;
                _chatClient.Connect();

                //Act
                await Task.Delay(100); //Le damos tiempo para que se ejecute el evento

                //Assert
                //Assert.True(eventRecived);
                monitoredSubject.Should().Raise("NewMessageRecived");
            }
        }

        [Fact]
        public async Task OverwriteLastLine_ShouldBeRaised_IfOneMessageAndAuthorIsWhoseIsLogged()
        {
            using (var monitoredSubject = _chatClient.Monitor())
            {
                //Arrange
                var database = TestDatabase.Instance();
                var user = await database.ChatUsers.FirstAsync(u => u.Name == "Usuario3");

                //var eventRecived = false;
                await _chatClient.LoginAsync(user.Name, user.Password);
                //_chatClient.OverwriteLastLine += (sender, _) => eventRecived = true;
                _chatClient.Connect();

                var message = new ChatMessage()
                {
                    Author = user.Name,
                    Message = "testing application message"
                };

                //Act
                await database.SendMessageAsync(message);  // sending message
                await Task.Delay(800); //Le damos tiempo para que se ejecute el evento

                //Assert
                //Assert.True(eventRecived);
                monitoredSubject.Should().Raise("OverwriteLastLine");
            }
        }

        [Fact]
        public async Task OverwriteLastLine_ShouldNotBeRaised_IfOneMessageAndAuthorIsOther()
        {
            using (var monitoredSubject = _chatClient.Monitor()){
                //Arrange
                var database = TestDatabase.Instance();

                var user = await database.ChatUsers.FirstAsync(u => u.Name == "Usuario1");
                var anotherUser = await database.ChatUsers.FirstAsync(u => u.Name == "Usuario3");

                //var eventRecived = false;
                await _chatClient.LoginAsync(user.Name, user.Password);
                //_chatClient.OverwriteLastLine += (sender, _) => eventRecived = true;
                _chatClient.Connect();


                var message = new ChatMessage()
                {
                    Author = anotherUser.Name,
                    Message = "testing application message"
                };

                //Act
                await database.SendMessageAsync(message);  // sending message
                await Task.Delay(1500); //Le damos tiempo para que se ejecute el evento

                //Assert
                //Assert.False(eventRecived);
                monitoredSubject.Should().NotRaise("OverwriteLastLine");
            }
            
        }

        [Fact]
        public async Task OverwriteLastLine_ShouldNotBeRaised_IfMoreThanOneMessage()
        {
            using (var monitoredSubject = _chatClient.Monitor()){
                //Arrange
                var database = TestDatabase.Instance();
                var user = await database.ChatUsers.FirstAsync(u => u.Name == "Usuario1");

                //var eventRecived = false;
                var messages = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        Author = user.Name,
                        Message = "Test message number 1",
                        Date = DateTime.Now
                    },
                    new ChatMessage
                    {
                        Author = user.Name,
                        Message = "Test message number 2",
                        Date = DateTime.Now
                    }
                };
                //Modificacion de Mock

                await _chatClient.LoginAsync(user.Name, user.Password);
                _chatClient.Connect();
                //_chatClient.OverwriteLastLine += (sender, _) => eventRecived = true;

                //Act
                await database.SendMessageAsync(messages[0]); // sending message 1
                await database.SendMessageAsync(messages[1]); // sending message 2
                await Task.Delay(500); //Le damos tiempo para que se ejecute el evento

                //Assert
                //Assert.False(eventRecived);
                monitoredSubject.Should().NotRaise("OverwriteLastLine");
            }
        }

        public void Dispose()
        {
            _chatClient?.Dispose();
        }

        public Task InitializeAsync() => Task.Delay(100);

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
