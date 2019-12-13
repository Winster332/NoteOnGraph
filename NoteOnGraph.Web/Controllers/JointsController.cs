using System;
using Microsoft.AspNetCore.Mvc;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;

namespace NoteOnGraph.Web.Controllers
{
    public class JointsController : Controller
    {
        private IRepository _repository;

        public JointsController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpPut]
        [Route("createJoint/{fromId}/{toId}")]
        public Guid CreateJoint(Guid fromId, Guid toId)
        {
            var joint = new Joint
            {
                Id = Guid.NewGuid(),
                From = fromId,
                To = toId
            };
            _repository.Create<Joint>(joint);
            
            return joint.Id;
        }
    }
}