using System.Collections.Generic;
using System.Threading.Tasks;
using NoteOnGraph.Client.Api.Common;
using NoteOnGraph.Client.Api.Extensions;
using NoteOnGraph.Models;

namespace NoteOnGraph.Client.Api.Endpoints
{
    public class NodesEndpointApi
    {
        private RestApi _rest;
        
        public NodesEndpointApi(RestApi rest)
        {
            _rest = rest;
        }
        
        public async Task<RequestResult<List<Project>>> GetProjectsAsync() =>
            await _rest.GetAsync<List<Project>>("/api/projects/getProjects", x => x.GetResponseData<List<Project>>());
    }
}