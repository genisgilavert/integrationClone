using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chat.DesignTests
{
    public class AssemblyReferenceTests : IClassFixture<AssemblyFixture>
    {
        private readonly AssemblyFixture _assemblyFixture;

        public AssemblyReferenceTests(AssemblyFixture assemblyFixture)
        {
            _assemblyFixture = assemblyFixture;
        }

        [Fact]
        public void ChatClientAssembly_ShouldReferenceChatClientLibraryAssembly_AndNoOthers()
        {
            //Arrange

            //Act

            //Assert
            _assemblyFixture.ChatClientAssembly.Should().Reference(_assemblyFixture.ChatClientLibraryAssembly);
            _assemblyFixture.ChatClientAssembly.Should().NotReference(_assemblyFixture.ChatCommonAssembly);
            _assemblyFixture.ChatClientAssembly.Should().NotReference(_assemblyFixture.ChatServerAssembly);
            _assemblyFixture.ChatClientAssembly.Should().NotReference(_assemblyFixture.ChatServerLibraryAssembly);
        }
        [Fact]
        public void ChatClientLibraryAssembly_ShouldReferenceChatCommon_AndNoOthers()
        {
            //Arrange

            //Act

            //Assert
            _assemblyFixture.ChatClientLibraryAssembly.Should().Reference(_assemblyFixture.ChatCommonAssembly);
            _assemblyFixture.ChatClientLibraryAssembly.Should().NotReference(_assemblyFixture.ChatClientAssembly);
            _assemblyFixture.ChatClientLibraryAssembly.Should().NotReference(_assemblyFixture.ChatServerAssembly);
            _assemblyFixture.ChatClientLibraryAssembly.Should().NotReference(_assemblyFixture.ChatServerLibraryAssembly);
        }

        [Fact]
        public void ChatServerAssembly_ShouldReferenceChatServerLibraryAssemblyAndChatCommonAssembly_AndNoOthers()
        {
            //Arrange

            //Act

            //Assert
            _assemblyFixture.ChatServerAssembly.Should().Reference(_assemblyFixture.ChatServerLibraryAssembly);
            _assemblyFixture.ChatServerAssembly.Should().Reference(_assemblyFixture.ChatCommonAssembly);
            _assemblyFixture.ChatServerAssembly.Should().NotReference(_assemblyFixture.ChatClientAssembly);
            _assemblyFixture.ChatServerAssembly.Should().NotReference(_assemblyFixture.ChatClientLibraryAssembly);
        }

        [Fact]
        public void ChatServerLibraryAssembly_ShouldReferenceChatCommonAssembly_AndNoOthers()
        {
            //Arrange

            //Act

            //Assert
            _assemblyFixture.ChatServerLibraryAssembly.Should().Reference(_assemblyFixture.ChatCommonAssembly);
            _assemblyFixture.ChatServerLibraryAssembly.Should().NotReference(_assemblyFixture.ChatClientAssembly);
            _assemblyFixture.ChatServerLibraryAssembly.Should().NotReference(_assemblyFixture.ChatClientLibraryAssembly);
            _assemblyFixture.ChatServerLibraryAssembly.Should().NotReference(_assemblyFixture.ChatServerAssembly);
        }

        [Fact]
        public void ChatCommonAssembly_ShouldNotReferenceAny()
        {
            //Arrange

            //Act

            //Assert
            _assemblyFixture.ChatCommonAssembly.Should().NotReference(_assemblyFixture.ChatClientAssembly);
            _assemblyFixture.ChatCommonAssembly.Should().NotReference(_assemblyFixture.ChatClientLibraryAssembly);
            _assemblyFixture.ChatCommonAssembly.Should().NotReference(_assemblyFixture.ChatServerAssembly);
            _assemblyFixture.ChatCommonAssembly.Should().NotReference(_assemblyFixture.ChatServerLibraryAssembly);
        }
    }
}
