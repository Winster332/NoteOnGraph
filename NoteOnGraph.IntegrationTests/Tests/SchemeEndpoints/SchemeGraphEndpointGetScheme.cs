using System;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.SchemeEndpoints
{
    public class SchemeGraphEndpointGetScheme : NoteOnGraphTest
    {
        [Fact]
        public async Task SchemeGraphEndpoint_GetScheme_ReturnScheme()
        {
            var file = new File
            {
                Id = Guid.NewGuid(),
                Href = "",
                Title = "File",
                Type = BlobType.File,
                SchemeId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid()
            };
            
            var responseCreate = await Api.Projects.CreateFileInRoot(file);
            var responseGetFile = await Api.Projects.GetFileInRootAsync(responseCreate.Result);
            file = responseGetFile.Result;

            var responseGetScheme = await Api.Schemes.GetSchemeGraph(file.SchemeId);
            responseGetScheme.Exception.Should().BeNull();
            responseGetScheme.Result.Should().NotBeNull();
        }
    }
}