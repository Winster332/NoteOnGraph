using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoteOnGraph.Client.Api.Common;
using NoteOnGraph.Client.Api.Extensions;
using NoteOnGraph.Models;

namespace NoteOnGraph.Client.Api.Endpoints
{
    public class ProjectsEndpointApi
    {
        private RestApi _rest;
        
        public ProjectsEndpointApi(RestApi rest)
        {
            _rest = rest;
        }
        
        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="project"></param>
        /// <returns>Project id</returns>
        public async Task<RequestResult<Guid>> CreateProjectInRootAsync(Project project) =>
            await _rest.PutAsync<Guid, Project>("/api/projects/createProjectInRoot", project,x => x.GetContentAsGuid());
        
        /// <summary>
        /// Return all projects
        /// </summary>
        /// <returns></returns>
        public async Task<RequestResult<List<Project>>> GetProjectsAsync() =>
            await _rest.GetAsync<List<Project>>("/api/projects/getProjects", x => x.GetResponseData<List<Project>>());
        
        /// <summary>
        /// Remove project
        /// </summary>
        /// <param name="projectId">Project id</param>
        /// <returns></returns>
        public async Task<RequestResult<string>> RemoveProjectAsync(Guid projectId) =>
            await _rest.DeleteAsync<string>("/api/projects/removeProject", projectId, x => string.Empty);
        
        /// <summary>
        /// Return project by id
        /// </summary>
        /// <param name="projectId">Project id</param>
        /// <returns></returns>
        public async Task<RequestResult<Project>> GetProjectAsync(Guid projectId) =>
            await _rest.GetAsync<Project>($"/api/projects/getProject/{projectId}", x => x.GetResponseData<Project>());
        
        
        
        
        /// <summary>
        /// Create file in root
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>File id</returns>
        public async Task<RequestResult<Guid>> CreateFileInRoot(File file) =>
            await _rest.PutAsync<Guid, File>("/api/projects/createFileInRoot", file, x => x.GetContentAsGuid());
        
        /// <summary>
        /// Return file in root
        /// </summary>
        /// <param name="fileId">File id</param>
        /// <returns></returns>
        public async Task<RequestResult<File>> GetFileInRootAsync(Guid fileId) =>
            await _rest.GetAsync<File>($"/api/projects/getFileInRoot/{fileId}", x => x.GetResponseData<File>());
        
        /// <summary>
        /// Return all files in root
        /// </summary>
        /// <returns></returns>
        public async Task<RequestResult<List<File>>> GetFilesInRootAsync() =>
            await _rest.GetAsync<List<File>>($"/api/projects/getFilesInRoot", x => x.GetResponseData<List<File>>());
        
        /// <summary>
        /// Remove file in root
        /// </summary>
        /// <param name="fileId">File id</param>
        /// <returns></returns>
        public async Task<RequestResult<string>> RemoveFileInRoot(Guid fileId) =>
            await _rest.DeleteAsync<string>($"/api/projects/removeFileInRoot", fileId, x => string.Empty);
        
        
        /// <summary>
        /// Create file in project
        /// </summary>
        /// <param name="projectId">Project id</param>
        /// <param name="file">File instance</param>
        /// <returns></returns>
        public async Task<RequestResult<Guid>> CreateFileInProject(Guid projectId, File file) =>
            await _rest.PutAsync<Guid, File>($"/api/projects/createFileInProject/{projectId}", file, x => x.GetContentAsGuid());
        
        /// <summary>
        /// Return file in project
        /// </summary>
        /// <param name="projectId">Project id</param>
        /// <param name="fileId">File id</param>
        /// <returns></returns>
        public async Task<RequestResult<File>> GetFileInProject(Guid projectId, Guid fileId) =>
            await _rest.GetAsync<File>($"/api/projects/getFileInProject/{projectId}/{fileId}", x => x.GetResponseData<File>());
        
        /// <summary>
        /// Remove file in project
        /// </summary>
        /// <param name="projectId">Project id</param>
        /// <param name="fileId">File id</param>
        /// <returns></returns>
        public async Task<RequestResult<string>> RemoveFileInProject(Guid projectId, Guid fileId) =>
            await _rest.DeleteAsync<string>($"/api/projects/removeFileInProject/{projectId}", fileId, x => string.Empty);
        
    }
}