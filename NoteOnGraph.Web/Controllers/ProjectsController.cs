using System;
using System.Collections.Generic;
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
        private IRepository _repository;

        public ProjectsController(IRepository repository)
        {
            _repository = repository;
//            _repository.Create<Project>(new Project
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "RootFolder",
//                    Type = BlobType.Folder,

//                    Files = new List<File>
//                    {
//                        new File
//                        {
//                            Id = Guid.NewGuid(),
//                            Href = "",
//                            Title = "File",
//                            Type = BlobType.File
//                        }
//                    }
//                });
        }

        [HttpPut]
        [Route("createProjectInRoot")]
        public Guid CreateProjectInRoot(Project project)
        {
            var id = Guid.NewGuid();
            project.Id = id;
            _repository.Create(project);

            return id;
        }
        
        [HttpDelete]
        [Route("removeProject/{id}")]
        public IActionResult RemoveProject(Guid id)
        {
            _repository.Delete<Project>(id);

            return Ok();
        }
        
        [HttpGet]
        [Route("getProjects")]
        public ActionResult<List<Project>> GetProjects()
        {
            return _repository.GetAll<Project>();
        }

        [HttpGet]
        [Route("getFiles")]
        public List<File> GetFiles()
        {
            return _repository.GetAll<File>();
        }
    }
}