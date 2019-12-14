using System;
using System.Threading.Tasks;
using NoteOnGraph.Client.Api.Common;
using NoteOnGraph.Client.Api.Extensions;
using NoteOnGraph.Models;

namespace NoteOnGraph.Client.Api.Endpoints
{
    public class SchemeGraphEndpointApi
    {
        private RestApi _rest;
        
        public SchemeGraphEndpointApi(RestApi rest)
        {
            _rest = rest;
        }
        
        public async Task<RequestResult<SchemeGraph>> GetSchemeGraph(Guid id) =>
            await _rest.GetAsync<SchemeGraph>($"/api/schemeGraph/getSchemeById/{id}", x => x.GetResponseData<SchemeGraph>());
    }
}