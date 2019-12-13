using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests
{
    public class ServerEndpointTests : NoteOnGraphTest
    {
        public ServerEndpointTests()
        {
        }
        [Fact]
        public async Task ServerEndpoint_GetVersion_ReturnSuccess()
        {
            var response = await Api.Get("/api/server/getVersion");
            
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);

            var version = await response.Content.ReadAsStringAsync();

            version.Should().BeEquivalentTo("1.0.0");
        }
    }
}