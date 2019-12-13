using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests
{
    public class ServerEndpointTests : NoteOnGraphTest
    {
        [Fact]
        public async Task ServerEndpoint_GetVersion_ReturnSuccess()
        {
            var response = await Client.GetAsync("/api/server/getVersion");
            
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);

            var version = await response.Content.ReadAsStringAsync();

            version.Should().BeEquivalentTo("1.0.0");
        }
    }
}