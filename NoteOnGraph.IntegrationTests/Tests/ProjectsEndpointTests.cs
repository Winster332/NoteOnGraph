using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NoteOnGraph.Models;
using Xunit;
using File = NoteOnGraph.Models.File;

namespace NoteOnGraph.IntegrationTests.Tests
{
    public class ProjectsEndpointTests : NoteOnGraphTest
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
                            Type = BlobType.File
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
                        Type = BlobType.File
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


        [Fact]
        public async Task ProjectsEndpointTest_CreateAndGetFileInRoot_ReturnSuccess()
        {
            var file = new File
            {
                Id = Guid.NewGuid(),
                Href = "",
                Title = "File",
                Type = BlobType.File
            };
            
            var responseCreate = await Api.Projects.CreateFileInRoot(file);
            responseCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseCreate.Exception.Should().BeNull();
            file.Id = responseCreate.Result;

            var responseGet = await Api.Projects.GetFileInRootAsync(file.Id);
            responseGet.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGet.Exception.Should().BeNull();
            responseGet.Result.Should().BeEquivalentTo(file);
        }

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
                    Type = BlobType.File
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


        [Fact]
        public async Task ProjectsEndpointTest_CreateFileInProject_ReturnSuccess()
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
                Type = BlobType.File
            };
            
            var responseProjectCreate = await Api.Projects.CreateProjectInRootAsync(project);
            project.Id = responseProjectCreate.Result;

            var responseFileCreate = await Api.Projects.CreateFileInProject(project.Id, file);
            responseFileCreate.Exception.Should().BeNull();
            responseFileCreate.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            file.Id = responseFileCreate.Result;

            var responseProject = await Api.Projects.GetProjectAsync(project.Id);
            project.Files.Add(file);
            
            responseProject.Result.Should().BeEquivalentTo(project);

            var responseGetFile = await Api.Projects.GetFileInProject(project.Id, file.Id);
            responseGetFile.Exception.Should().BeNull();
            responseGetFile.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseGetFile.Result.Should().BeEquivalentTo(file);
        }

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
                Type = BlobType.File
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