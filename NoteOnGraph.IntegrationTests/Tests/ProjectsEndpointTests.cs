using System;
using System.Collections.Generic;
using System.IO;
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
            var responseCreate = await Api.Put("api/projects/createProjectInRoot", new Project
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
            });
                
            responseCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            
            var responseGet = await Api.Get("api/projects/getProjects");
            
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            
            var projects = JsonConvert.DeserializeObject<List<Project>>(await responseGet.Content.ReadAsStringAsync());
            Console.WriteLine("123");
        }

        public void Dispose()
        {
            Directory.Delete("db", true);
        }
    }
}