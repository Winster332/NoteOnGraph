using System;
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
        
        public async Task<RequestResult<Guid>> CreateNode(Node node) =>
            await _rest.PutAsync<Guid, Node>("/api/nodes/createNode", node,x => x.GetContentAsGuid());
        
        public async Task<RequestResult<List<Node>>> GetNodes() =>
            await _rest.GetAsync<List<Node>>("/api/nodes/getNodes", x => x.GetResponseData<List<Node>>());
        
        public async Task<RequestResult<Node>> GetNodeById(Guid id) =>
            await _rest.GetAsync<Node>($"/api/nodes/getNodeById/{id}", x => x.GetResponseData<Node>());
        
        public async Task<RequestResult<NodeData>> GetDataByNodeId(Guid id) =>
            await _rest.GetAsync<NodeData>($"/api/nodes/getData/{id}", x => x.GetResponseData<NodeData>());
        
        public async Task<RequestResult<string>> RemoveNode(Guid id) =>
            await _rest.DeleteAsync<string>($"/api/nodes/removeNode", id, x => string.Empty);
    }
}