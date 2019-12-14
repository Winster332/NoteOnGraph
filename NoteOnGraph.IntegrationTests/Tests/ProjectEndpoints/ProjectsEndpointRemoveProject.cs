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
    public class ProjectsEndpointRemoveProject : NoteOnGraphTest
    {
        [Fact]
        public async Task ProjectsEndpointTest_RemoveProject_ReturnSuccess()
        {
            var currentProjects = new List<Project>();
            
            for (var i = 0; i < 5; i++)
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
                
                currentProjects.Add(project);
            }

            var currentProject = currentProjects.FirstOrDefault();
            
            var responseRemove = await Api.Projects.RemoveProjectAsync(currentProject.Id);
            responseRemove.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseRemove.Exception.Should().BeNull();

            var responseGet = await Api.Projects.GetProjectsAsync();
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGet.Exception.Should().BeNull();
            responseGet.Result.Should().HaveCount(4);

            var assertionResponse = await Api.Projects.GetProjectAsync(currentProject.Id);
            assertionResponse.Exception.Should().BeNull();
            assertionResponse.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            assertionResponse.Result.Should().BeNull();
        }
    }
}