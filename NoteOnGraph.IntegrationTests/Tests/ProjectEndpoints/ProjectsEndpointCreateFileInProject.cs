using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.ProjectEndpoints
{
    public class ProjectsEndpointCreateFileInProject : NoteOnGraphTest
    {
        [Fact]
        public async Task ProjectsEndpointTest_CreateFileInProject_ReturnSuccess()
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = "RootFolder",
                Type = BlobType.Folder,
                Files = new List<File>(),
            };
            var file = new File
            {
                Id = Guid.NewGuid(),
                Href = "",
                Title = "File",
                Type = BlobType.File,
                SchemeId = Guid.Empty,
                ProjectId = Guid.Empty
            };
            
            var responseProjectCreate = await Api.Projects.CreateProjectInRootAsync(project);
            project.Id = responseProjectCreate.Result;

            var responseFileCreate = await Api.Projects.CreateFileInProject(project.Id, file);
            responseFileCreate.Exception.Should().BeNull();
            responseFileCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);

            var responseFile = await Api.Projects.GetFileInProject(project.Id, responseFileCreate.Result);
            
            file.Id = responseFileCreate.Result;
            file.ProjectId = project.Id;
            file.SchemeId = responseFile.Result.SchemeId;

            var responseProject = await Api.Projects.GetProjectAsync(project.Id);
            project.Files.Add(file);
            
            responseProject.Result.Should().BeEquivalentTo(project);

            var responseGetFile = await Api.Projects.GetFileInProject(project.Id, file.Id);
            responseGetFile.Exception.Should().BeNull();
            responseGetFile.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGetFile.Result.Should().BeEquivalentTo(file);
        }
    }
}