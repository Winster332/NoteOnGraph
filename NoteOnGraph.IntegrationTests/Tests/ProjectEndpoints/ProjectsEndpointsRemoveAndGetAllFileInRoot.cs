using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.ProjectEndpoints
{
    public class ProjectsEndpointsRemoveAndGetAllFileInRoot : NoteOnGraphTest
    {
        [Fact]
        public async Task ProjectsEndpointTest_RemoveAndGetAllFileInRoot_ReturnSuccess()
        {
            var files = new List<File>();
            
            for (var i = 0; i < 5; i++)
            {
                var file = new File
                {
                    Id = Guid.NewGuid(),
                    Href = "",
                    Title = "File",
                    Type = BlobType.File,
                    ProjectId = Guid.Empty,
                    SchemeId = Guid.Empty
                };

                var responseCreate = await Api.Projects.CreateFileInRoot(file);
                responseCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
                responseCreate.Exception.Should().BeNull();
                file.Id = responseCreate.Result;
                
                files.Add(file);
            }

            var currentFile = files.FirstOrDefault();

            var responseGetAll1 = await Api.Projects.GetFilesInRootAsync();
            responseGetAll1.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGetAll1.Exception.Should().BeNull();
            responseGetAll1.Result.Should().HaveCount(5);

            var responeRemove = await Api.Projects.RemoveFileInRoot(currentFile.Id);
            responeRemove.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responeRemove.Exception.Should().BeNull();
            responeRemove.Result.Should().BeEquivalentTo(string.Empty);
            
            var responseGetAll2 = await Api.Projects.GetFilesInRootAsync();
            responseGetAll2.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGetAll2.Exception.Should().BeNull();
            responseGetAll2.Result.Should().HaveCount(4);
            
            var respone = await Api.Projects.GetFileInRootAsync(currentFile.Id);
            respone.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            respone.Exception.Should().BeNull();
            respone.Result.Should().BeNull();
        }
    }
}