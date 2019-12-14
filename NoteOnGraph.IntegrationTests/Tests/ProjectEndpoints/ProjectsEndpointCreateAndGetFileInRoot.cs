using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.ProjectEndpoints
{
    public class ProjectsEndpointCreateAndGetFileInRoot : NoteOnGraphTest
    {
        [Fact]
        public async Task ProjectsEndpointTest_CreateAndGetFileInRoot_ReturnSuccess()
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
            responseCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseCreate.Exception.Should().BeNull();
            file.Id = responseCreate.Result;

            var externalFileResponse = await Api.Projects.GetFileInRootAsync(file.Id);
            var entityExternalFile = externalFileResponse.Result;

            file.SchemeId = entityExternalFile.SchemeId;
            file.ProjectId = entityExternalFile.ProjectId;

            var responseGet = await Api.Projects.GetFileInRootAsync(file.Id);
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGet.Exception.Should().BeNull();
            responseGet.Result.Should().BeEquivalentTo(file);
        }
    }
}