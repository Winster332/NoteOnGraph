using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        [HttpPut("CreateProjectInRoot")]
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
        [Route("getProject/{id}")]
        public ActionResult<Project> GetProject(Guid id)
        {
            return _repository.Read<Project>(id);
        }
        
        [HttpGet]
        [Route("getProjects")]
        public ActionResult<List<Project>> GetProjects()
        {
            return _repository.GetAll<Project>();
        }

        
        // Files
        
        [HttpPut]
        [Route("createFileInRoot")]
        public Guid CreateFileInRoot(File file)
        {
            var id = Guid.NewGuid();
            file.Id = id;
            file.SchemeId = Guid.NewGuid();
            file.ProjectId = Guid.Empty;
            _repository.Create<File>(file);

            _repository.Create<SchemeGraph>(new SchemeGraph
            {
                Id = file.SchemeId,
                FileId = file.Id,
                
                Nodes = new List<Guid>(),
                Joints = new List<Guid>()
            });
            
            return id;
        }
        
        [HttpGet]
        [Route("getFileInRoot/{id}")]
        public File GetFileInRoot(Guid id)
        {
            var file = _repository.Read<File>(id);

            return file;
        }
        
        [HttpGet]
        [Route("getFilesInRoot")]
        public List<File> GetFilesInRoot()
        {
            var files = _repository.GetAll<File>();

            return files;
        }
        
        [HttpDelete]
        [Route("removeFileInRoot/{id}")]
        public IActionResult RemoveFileInRoot(Guid id)
        {
            _repository.Delete<File>(id);
            
            return Ok();
        }
        
        [HttpPut]
        [Route("createFileInProject/{projectId}")]
        public Guid CreateFileInProject(Guid projectId, File file)
        {
            var id = Guid.NewGuid();
            file.Id = id;
            file.ProjectId = projectId;

            var scheme = new SchemeGraph
            {
                Id = Guid.NewGuid(),
                FileId = file.Id,

                Nodes = new List<Guid>(),
                Joints = new List<Guid>()
            };

            file.SchemeId = scheme.Id;
            
            _repository.Create<SchemeGraph>(scheme);
            

            var project = _repository.Read<Project>(projectId);

            if (project == null)
            {
                return Guid.Empty;
            }
            
            project.Files.Add(file);
            
            _repository.Update<Project>(project);
            
            return id;
        }
        
        [HttpGet]
        [Route("getFileInProject/{projectId}/{fileId}")]
        public File GetFileInProject(Guid projectId, Guid fileId)
        {
            var file = _repository.Read<Project>(projectId).Files.FirstOrDefault(x => x.Id == fileId);

            return file;
        }
        
        [HttpDelete]
        [Route("removeFileInProject/{projectId}/{fileId}")]
        public ActionResult RemoveFileInProject(Guid projectId, Guid fileId)
        {
            var project = _repository.Read<Project>(projectId);
            var fileIndex = -1;

            if (project == null)
            {
                return BadRequest($"Not found project by id {projectId}");
            }
            
            for (var i = 0; i < project.Files.Count; i++)
            {
                if (project.Files[i].Id == fileId)
                {
                    fileIndex = i;
                    break;
                }
            }

            if (fileIndex == -1)
            {
                return BadRequest($"Not found file by id {fileId}");
            }

            project.Files.RemoveAt(fileIndex);
            
            _repository.Update<Project>(project);

            return Ok();
        }
    }
}