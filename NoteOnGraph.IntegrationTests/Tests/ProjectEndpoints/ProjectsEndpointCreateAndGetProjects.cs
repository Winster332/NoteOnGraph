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
    public class ProjectsEndpointCreateAndGetProjects : NoteOnGraphTest
    {
        [Fact]
        public async Task ProjectsEndpointTest_CreateAndGetProjects_ReturnSuccess()
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = "RootFolder",
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
            responseCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseCreate.Exception.Should().BeNull();
            
            project.Id = responseCreate.Result;

            var responseGet = await Api.Projects.GetProjectsAsync();
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGet.Exception.Should().BeNull();

            var projects = responseGet.Result;
            projects.Should().HaveCount(1);
            projects.FirstOrDefault().Should().BeEquivalentTo(project);
        }
    }
}