using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NoteOnGraph.Models;
using Xunit;
using File = NoteOnGraph.Models.File;

namespace NoteOnGraph.IntegrationTests.Tests
{
    public class ProjectsEndpointTests : NoteOnGraphTest, IDisposable
    {
        public ProjectsEndpointTests()
        {
        }
        
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
                        Type = BlobType.File
                    }
                }
            };
            
            var responseCreate = await Api.Projects.CreateProjectInRoot(project);//"api/projects/createProjectInRoot", 
            project.Id = responseCreate.Result;
            responseCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);

            var responseGet = await Api.Projects.GetProjects();//.Get("api/projects/getProjects");
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGet.Exception.Should().BeNull();

            var projects = responseGet.Result;// responseGet.GetResponseDataAsync<List<Project>>();
            projects.Should().HaveCount(1);
            projects.FirstOrDefault().Should().BeEquivalentTo(project);
        }

        [Fact]
        public async Task ProjectsEndpointTest_RemoveProject_ReturnSuccess()
        {
        }

        public void Dispose()
        {
            Directory.Delete("db", true);
        }
    }
}