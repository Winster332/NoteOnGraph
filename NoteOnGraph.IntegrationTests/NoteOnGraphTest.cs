using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NoteOnGraph.Client.Api;
using NoteOnGraph.Web;

namespace NoteOnGraph.IntegrationTests
{
    public class NoteOnGraphTest
    {
        public NoteOnGraphClient Api { get; set; }
        public RestAPI Rest { get; set; }
        private TestServer _testServer;
        
        public NoteOnGraphTest()
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(x =>
                {
                    x.AddAutofac(builder =>
                    {
                    });
                })
                .Configure(app =>
                {
                    Console.WriteLine("123");
                })
                .UseStartup<Startup>();
            
            _testServer = new TestServer(hostBuilder);
            var client = _testServer.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            
            Rest = new RestAPI(client);
            Api = new NoteOnGraphClient(client);
        }
    }
}