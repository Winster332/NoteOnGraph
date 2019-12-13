using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Autofac.Integration.WebApi;
using Microsoft.AspNetCore.Mvc;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;

namespace NoteOnGraph.Web.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [AutofacControllerConfiguration]
    public class ProjectsController : ControllerBase
    {
        private IRepository _projects;

        public ProjectsController(IRepository repository)
        {
            _projects = repository;
            _projects.Create<Project>(new Project
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
        }

        [HttpPut]
        [Route("createProjectInRoot")]
        public IActionResult CreateProjectInRoot(Project project)
        {
            _projects.Create(project);

            return Ok();
        }
        
        [HttpDelete]
        [Route("removeProject/{id}")]
        public IActionResult RemoveProject(Guid id)
        {
            _projects.Delete<Project>(id);

            return Ok();
        }
        
        [HttpGet]
        [Route("getProjects")]
        public ActionResult<List<Project>> GetProjects()
        {
            return _projects.GetAll<Project>();
        }

        [HttpGet]
        [Route("getFiles")]
        public List<File> GetFiles()
        {
            return new List<File>
            {
                new File
                {
                    Id = Guid.NewGuid(),
                    Href = "",
                    Title = "File",
                    Type = BlobType.File
                }
            };
        }
    }
}