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
        
        [HttpGet]
        [Route("getNodes")]
        public List<Node> GetNodes()
        {
            var nodes = _repository.GetAll<Node>();
            return nodes;
        }
    }
}