using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;

namespace NoteOnGraph.Web.Controllers
{
    [ApiController]
    [Route("api/joints")]
    public class JointsController : ControllerBase
    {
        private IRepository _repository;

        public JointsController(IRepository repository)
        {
            _repository = repository;
        }
        
        [HttpPut]
        [Route("createJoint")]
        public Guid CreateJoint(Joint joint)
        {
            var jointNew = new Joint
            {
                Id = Guid.NewGuid(),
                From = joint.From,
                To = joint.To
            };
            _repository.Create<Joint>(jointNew);
            
            var nodeFrom = _repository.Read<Node>(jointNew.From);
            var nodeTo = _repository.Read<Node>(jointNew.To);

            nodeFrom.Outputs.Add(jointNew.Id);
            nodeTo.Inputs.Add(jointNew.Id);
            
            _repository.Update(nodeFrom);
            _repository.Update(nodeTo);
            
            return jointNew.Id;
        }
        
        [HttpGet]
        [Route("getJoints")]
        public List<Joint> GetJoints()
        {
            return _repository.GetAll<Joint>();
        }
        
        [HttpGet]
        [Route("getJointById/{id}")]
        public Joint GetJointById(Guid id)
        {
            return _repository.Read<Joint>(id);
        }
        
        [HttpDelete]
        [Route("removeJoint/{jointId}")]
        public ActionResult RemoveJoint(Guid jointId)
        {
            var joint = _repository.Read<Joint>(jointId);

            var nodeFrom = _repository.Read<Node>(joint.From);
            var nodeTo = _repository.Read<Node>(joint.To);

            nodeFrom.Outputs.Remove(joint.Id);
            nodeTo.Inputs.Remove(joint.Id);
            
            _repository.Update(nodeFrom);
            _repository.Update(nodeTo);
            
            _repository.Delete<Joint>(joint.Id);
            
            return Ok();
        }
    }
}