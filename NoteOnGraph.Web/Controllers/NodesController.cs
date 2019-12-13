using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;

namespace NoteOnGraph.Web.Controllers
{
    [ApiController]
    [Route("api/nodes")]
    public class NodesController : ControllerBase
    {
        private IRepository _repository;

        public NodesController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpPut]
        [Route("createNode")]
        public Guid CreateNode(Node node)
        {
            node.Id = Guid.NewGuid();
            _repository.Create<Node>(node);

            return node.Id;
        }
        
        [HttpGet]
        [Route("getNodes")]
        public List<Node> GetNodes()
        {
            var nodes = _repository.GetAll<Node>();
            return nodes;
        }
        
        [HttpGet]
        [Route("getNodeById/{nodeId}")]
        public Node GetNodeById(Guid nodeId)
        {
            var node = _repository.Read<Node>(nodeId);
            return node;
        }
        
        [HttpDelete]
        [Route("removeNode/{nodeId}")]
        public ActionResult RemoveNode(Guid nodeId)
        {
            _repository.Delete<Node>(nodeId);
            
            return Ok();
        }
    }
}