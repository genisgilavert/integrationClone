using Chat.Common.Entities;
using Chat.FitxureServer.Library.FitxureData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Chat.FitxureServer.Library.Services
{
    public class FakeMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            string str = request.RequestUri.Segments[1];

            if (str.Equals("ChatMessages"))
            {
                var chatMessages = TestDatabase.Instance().ChatMessages.ToList();
                response = new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(chatMessages), Encoding.UTF8, "application/json")
                };

                /*
                ChatUser chatUser = new ChatUser();

                response = new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(chatUser), Encoding.UTF8, "application/json")
                };
                */
            } else if (str.Equals("ChatUsers")) {

                var query = request.RequestUri.Query;
                var queryString = HttpUtility.ParseQueryString(query);
                string name = queryString["username"];
                string password = queryString["password"];

                ChatUser chatUser = new ChatUser() {
                    Name = name,
                    Password = password
                };

                response = new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(chatUser), Encoding.UTF8, "application/json")
                };
            }else{
                response = new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            return Task.FromResult(response);
        }

    }

}
