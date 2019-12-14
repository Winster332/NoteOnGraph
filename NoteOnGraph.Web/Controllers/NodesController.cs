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
            node.DataId = Guid.NewGuid();

            var data = new NodeData
            {
                Id = node.DataId,
                NodeId = node.Id,
                Text = string.Empty
            };
            
            _repository.Create<NodeData>(data);
            
            _repository.Create<Node>(node);
            
            var scheme = _repository.Read<SchemeGraph>(node.SchemeId);
            scheme.Nodes.Add(node.Id);
            
            _repository.Update(scheme);

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
            var node = _repository.Read<Node>(nodeId);
            
            _repository.Delete<NodeData>(node.DataId);
            _repository.Delete<Node>(nodeId);
            
            return Ok();
        }
        
        [HttpGet]
        [Route("getData/{nodeId}")]
        public NodeData GetDataByNodeId(Guid nodeId)
        {
            var node = _repository.Read<Node>(nodeId);

            if (node == null)
            {
                return null;
            }

            var data = _repository.Read<NodeData>(node.DataId);
            
            return data;
        }
    }
}