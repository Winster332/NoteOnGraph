using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NoteOnGraph.Domains.Aggregators;
using NoteOnGraph.Services;

namespace NoteOnGraph.WebApi.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        public INodeService NodeService { get; set; }
        public IMongoDatabase Database { get; set; }
        
        public NodesController(IMongoDatabase database, INodeService nodeService)
        {
            NodeService = nodeService;
            Database = database;
        }
        
        [Route("entities/get/{nodeId}")]
        [HttpGet]
        public async Task<Node> GetNode(Guid nodeId)
        {
            var node = await NodeService.GetNodeAsync(nodeId);
            
            return node;
        }
    }
}