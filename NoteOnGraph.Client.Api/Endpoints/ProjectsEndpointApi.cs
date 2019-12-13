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
        
        public async Task<RequestResult<Guid>> CreateProjectInRoot(Project project)
        {
            var response = await _rest.PutAsync<Guid, Project>("/api/projects/createProjectInRoot", project,
                res => res.GetContentAsGuid());

            return response;
        }
        
        public async Task<RequestResult<List<Project>>> GetProjects()
        {
            var response = await _rest.GetAsync<List<Project>>("/api/projects/getProjects", x => x.GetResponseData<List<Project>>());

            return response;
        }
    }
}