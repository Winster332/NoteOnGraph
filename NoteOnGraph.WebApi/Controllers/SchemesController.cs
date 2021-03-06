using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NoteOnGraph.Domains;
using NoteOnGraph.Services;
using NoteOnGraph.WebApi.Models;

namespace NoteOnGraph.WebApi.Controllers
{
    [Route("api/schemes")]
    [ApiController]
    public class SchemesController : ControllerBase
    {
        public ISchemeService SchemeService { get; set; }
        public IMongoDatabase Database { get; set; }
        
        public SchemesController(IMongoDatabase database, ISchemeService schemeService)
        {
            SchemeService = schemeService;
            Database = database;
        }
        
        [Route("getSchemes")]
        [HttpGet]
        public async Task<IList<Scheme>> GetSchemes()
        {
            var schemes = await SchemeService.GetSchemesAsync();
            
            return schemes;
        }
        
        [Route("getScheme/{schemeId}")]
        [HttpGet]
        public async Task<ActionResult<Scheme>> GetScheme(Guid schemeId)
        {
            var scheme = await SchemeService.GetSchemeAsync(schemeId);

            return scheme;
        }
        
        [Route("createScheme")]
        [HttpPut]
        public async Task<Scheme> CreateScheme()
        {
            var scheme = await SchemeService.CreateSchemeAsync();

            return scheme;
        }

        [Route("removeScheme/{schemeId}")]
        [HttpDelete]
        public async Task RemoveScheme(Guid schemeId)
        {
            await SchemeService.RemoveSchemeAsync(schemeId);
        }
        
        [Route("nodes/create")]
        [HttpPut]
        public async Task<Node> CreateNode([FromBody] CreateNodeModel nodeModel)
        {
            var node = await SchemeService.CreateNodeOnScheme(nodeModel.SchemeId, nodeModel.X, nodeModel.Y, string.Empty);

            return node;
        }
        
        [Route("joints/create")]
        [HttpPut]
        public async Task<Joint> CreateJoint([FromBody] CreateJointModel jointModel)
        {
            var joint = await SchemeService.CreateJointOnSchemeAsync(jointModel.SchemeId, jointModel.NodeFromId, jointModel.NodeToId);

            return joint;
        }
        
        [Route("{schemeId}/nodes/remove/{nodeId}")]
        [HttpDelete]
        public async Task RemoveNode(Guid schemeId, Guid nodeId)
        {
            await SchemeService.RemoveNodeFromScheme(schemeId, nodeId);
        }
        
        [Route("{schemeId}/joints/remove/{jointId}")]
        [HttpDelete]
        public async Task RemoveJoint(Guid schemeId, Guid jointId)
        {
            await SchemeService.RemoveJointOnSchemeAsync(schemeId, jointId);
        }
        
//        [HttpPost]
//        public void Post([FromBody] string value)
//        {
//        }
    }
}