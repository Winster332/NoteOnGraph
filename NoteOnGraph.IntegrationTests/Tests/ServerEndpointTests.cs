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
        public async Task ServerEndpoint_GetInfo_ReturnSuccess()
        {
            var response = await Api.Server.GetInfo();
            
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            response.Exception.Should().BeNull();

            var info = response.Result;

            info.Version.Should().BeEquivalentTo("1.0.0");
        }
    }
}