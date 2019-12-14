using System;
using System.Collections.Generic;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;

namespace NoteOnGraph.Services
{
    public interface IProjectService
    {
        Project CreateProject(string title);
        void RemoveProject(Guid projectId);
        Project GetProjectById(Guid id);
        IList<Project> GetProjects();

    }

    public class ProjectService : IProjectService
    {
        private IRepository _repository;
        
        public ProjectService(IRepository repository)
        {
            _repository = repository;
        }

        public Project CreateProject(string title)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = title,
                Type = BlobType.Folder,
                Files = new List<File>()
            };
            
            _repository.Create(project);

            return project;
        }

        public void RemoveProject(Guid projectId)
        {
            var project = _repository.Read<Project>(projectId);
            
            for (var i = 0; i < project.Files.Count; i++)
            {
                var file = project.Files[i];
                
                _repository.Delete<File>(file.Id);
                _repository.Delete<SchemeGraph>(file.SchemeId);
            }
            
            _repository.Delete<Project>(project.Id);
        }

        public Project GetProjectById(Guid id)
        {
            return _repository.Read<Project>(id);
        }

        public IList<Project> GetProjects()
        {
            return _repository.GetAll<Project>();
        }
    }
}