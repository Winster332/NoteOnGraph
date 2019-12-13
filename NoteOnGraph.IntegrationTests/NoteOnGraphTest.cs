using System;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NoteOnGraph.Web;

namespace NoteOnGraph.IntegrationTests
{
    public class NoteOnGraphTest
    {
        public HttpClient Client { get; set; }
        private TestServer _testServer;
        
        public NoteOnGraphTest()
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(x => x.AddAutofac())
                .UseStartup<Startup>();
            
            _testServer = new TestServer(hostBuilder);
            Client = _testServer.CreateClient();
            Client.BaseAddress = new Uri("https://localhost:5001");
        }
    }
}