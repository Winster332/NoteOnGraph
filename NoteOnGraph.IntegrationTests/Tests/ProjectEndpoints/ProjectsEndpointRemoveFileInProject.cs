using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.ProjectEndpoints
{
    public class ProjectsEndpointRemoveFileInProject : NoteOnGraphTest
    {
        [Fact]
        public async Task ProjectsEndpointTest_RemoveFileInProject_ReturnSuccess()
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = "RootFolder",
                Type = BlobType.Folder,
                Files = new List<File>()
            };
            var file = new File
            {
                Id = Guid.NewGuid(),
                Href = "",
                Title = "File",
                Type = BlobType.File,
                ProjectId = Guid.Empty,
                SchemeId = Guid.Empty
            };
            
            var responseProjectCreate = await Api.Projects.CreateProjectInRootAsync(project);
            project.Id = responseProjectCreate.Result;

            var responseFileCreate = await Api.Projects.CreateFileInProject(project.Id, file);
            file.Id = responseFileCreate.Result;

            await Api.Projects.RemoveFileInProject(project.Id, file.Id);
            
            var responseProject = await Api.Projects.GetProjectAsync(project.Id);
            
            responseProject.Result.Files.Should().HaveCount(0);

            var responseGetFile = await Api.Projects.GetFileInProject(project.Id, file.Id);
            responseGetFile.Exception.Should().BeNull();
            responseGetFile.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            responseGetFile.Result.Should().BeNull();
        }
    }
}