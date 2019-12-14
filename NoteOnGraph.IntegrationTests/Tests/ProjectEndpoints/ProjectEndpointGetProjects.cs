using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.IntegrationTests.Tests.ProjectEndpoints
{
    public class ProjectEndpointGetProjects : NoteOnGraphTest
    {
        [Fact]
        public async Task ProjectsEndpointTest_GetProject_ReturnSuccess()
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = $"RootFolder-",
                Type = BlobType.Folder,

                Files = new List<File>
                {
                    new File
                    {
                        Id = Guid.NewGuid(),
                        Href = "",
                        Title = "File",
                        Type = BlobType.File,
                        SchemeId = Guid.NewGuid(),
                        ProjectId = Guid.NewGuid()
                    }
                }
            };

            var responseCreate = await Api.Projects.CreateProjectInRootAsync(project);
            project.Id = responseCreate.Result;

            var responseGet = await Api.Projects.GetProjectAsync(project.Id);
            responseGet.Exception.Should().BeNull();
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGet.Result.Should().BeEquivalentTo(project);
        }
    }
}