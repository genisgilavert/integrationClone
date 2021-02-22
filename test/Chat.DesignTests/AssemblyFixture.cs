using System;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Chat.Common.Entities;

namespace Chat.DesignTests
{
    public class AssemblyFixture
    {
        public Assembly ChatClientAssembly { get; private set; }
        public Assembly ChatClientLibraryAssembly { get; private set; }
        public Assembly ChatCommonAssembly { get; private set; }
        public Assembly ChatServerAssembly { get; private set; }
        public Assembly ChatServerLibraryAssembly { get; private set; }

        public AssemblyFixture()
        {
            ChatClientAssembly = typeof(Client.Program).Assembly;
            ChatClientLibraryAssembly = typeof(Client.Library.Services.ChatClient).Assembly;
            ChatCommonAssembly = typeof(Common.Entities.ChatMessage).Assembly;
            ChatServerAssembly = typeof(Server.Program).Assembly;
            ChatServerLibraryAssembly = typeof(Server.Library.Data.ChatDbContext).Assembly;
        }

    }
}
