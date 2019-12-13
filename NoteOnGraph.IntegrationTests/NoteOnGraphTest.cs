using System;
using System.Net.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NoteOnGraph.IntegrationTests.Tests;
using NoteOnGraph.Web;
using Xunit;

namespace NoteOnGraph.IntegrationTests
{
    public class NoteOnGraphTest
    {
        public RestAPI Api { get; set; }
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
            
            Api = new RestAPI(client);
        }
    }
}