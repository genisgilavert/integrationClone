using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Chat.Client.Library.Services;
using Chat.FitxureServer.Library.Services;
using System.Net.Http;
using Chat.Common.Entities;

namespace Chat.Client.Library.Tests
{
    public class UserApiClientTests
    {
        private readonly UserApiClient _userApiClient;
        public UserApiClientTests(){
            var httpClient = new HttpClient(new FakeMessageHandler()) { 
                BaseAddress = new Uri("http://localhost")
            };
            _userApiClient = new UserApiClient(httpClient);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldBeExpectResult(){

            //Arrange
            var expected = new { name = "Usuario1", password = "P2ssw0rd!" };
            //Act
            ChatUser result = await _userApiClient.CreateUserAsync(expected.name, expected.password);
            //Assert
            result.Name.Should().BeEquivalentTo(expected.name);
            result.Password.Should().BeEquivalentTo(expected.password);
        }

        [Fact]
        public async Task LoginAsync_ShouldBeExpectResult(){

            //Arrange
            var expected = new { name = "Usuario1", password = "P2ssw0rd!" };
            //Act
            ChatUser result = await _userApiClient.LoginAsync(expected.name, expected.password);
            //Assert
            result.Name.Should().BeEquivalentTo(expected.name);
            result.Password.Should().BeEquivalentTo(expected.password);
        }
    }
}
