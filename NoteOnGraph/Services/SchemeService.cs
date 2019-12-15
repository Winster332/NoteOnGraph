using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NoteOnGraph.Domains.Aggregators;
using Serilog;

namespace NoteOnGraph.Services
{
    public interface ISchemeService
    {
    }

    public class SchemeService : ISchemeService
    {
        private IMongoCollection<Scheme> _schemes;
        private IMongoCollection<Joint> _joints;
        private INodeService _nodeService;
        
        public SchemeService(IMongoDatabase database, INodeService nodeService)
        {
            _schemes = database.GetCollection<Scheme>("schemes");
            _nodeService = nodeService;
            _joints = database.GetCollection<Joint>("joints");
        }
        
        public async Task<Scheme> CreateSchemeAsync()
        {
            var scheme = new Scheme
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Removed = false,
                CreatedDateTime = DateTime.Now,
                ChangeDateTime = DateTime.Now,

                JointsIds = new List<Guid>(),
                NodesIds = new List<Guid>(),

            };
            
            await _schemes.InsertOneAsync(scheme);

            return scheme;
        }
        
        public async Task AddNodeOnSchemeAsync(Guid schemeId, Guid nodeId)
        {
            var scheme = await GetSchemeAsync(schemeId);
            scheme.NodesIds.Add(nodeId);
            
            var update = Builders<Scheme>.Update
                .Set(x => x.NodesIds, scheme.NodesIds)
                .Set(x => x.ChangeDateTime, DateTime.Now);
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);
            
            await _schemes.UpdateOneAsync(filter, update);
        }

        public async Task<Node> CreateNodeOnScheme(Guid schemeId, float x, float y, string caption)
        {
            var node = await _nodeService.Create(x, y, caption);
            
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);

            var schemes = await _schemes.FindAsync(filter);
            var scheme = schemes.FirstOrDefault();

            scheme.NodesIds.Add(node.Id);
            
            var update = Builders<Scheme>.Update
                .Set(x => x.NodesIds, scheme.NodesIds)
                .Set(x => x.ChangeDateTime, DateTime.Now);

            await _schemes.UpdateOneAsync(filter, update);

            return node;
        }
        
        public async Task<bool> RemoveNode(Guid schemeId, Guid nodeId)
        {
            var isRemoved = false;

            try
            {
                await _nodeService.RemoveAsync(nodeId);

                var filterScheme = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);

                var schemes = await _schemes.FindAsync(filterScheme);
                var scheme = schemes.FirstOrDefault();

                scheme.NodesIds.Remove(nodeId);

                var update = Builders<Scheme>.Update
                    .Set(x => x.NodesIds, scheme.NodesIds)
                    .Set(x => x.ChangeDateTime, DateTime.Now);

                await _schemes.UpdateOneAsync(filterScheme, update);
                isRemoved = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

                isRemoved = false;
            }

            return isRemoved;
        }
        
        public async Task<Joint> AddJoint(Guid schemeId, Guid nodeFromId, Guid nodeToId)
        {
            var joint = new Joint(_joints);
            await joint.Create(nodeFromId, nodeToId);
            
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);

            var schemes = await _schemes.FindAsync(filter);
            var scheme = schemes.FirstOrDefault();

            await scheme.AddJoint(joint.Id);

            return joint;
        }
        
        public async Task<bool> RemoveJoint(Guid schemeId, Guid jointId)
        {
            var isRemoved = false;
            
            try
            {
                var filterJoint = Builders<Joint>.Filter.Eq(x => x.Id, jointId);
                await _joints.DeleteOneAsync(filterJoint);

                var filterScheme = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);

                var schemes = await _schemes.FindAsync(filterScheme);
                var scheme = schemes.FirstOrDefault();

                await scheme.RemoveJoint(jointId);
                isRemoved = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                
                isRemoved = false;
            }

            return isRemoved;
        }

        public async Task<Scheme> GetSchemeAsync(Guid schemeId)
        {
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);
            
            var schemes = await _schemes.FindAsync(filter);

            return schemes.FirstOrDefault();
        }

        public async Task<IList<Scheme>> GetSchemesAsync()
        {
            var filter = Builders<Scheme>.Filter.Eq(x => x.Removed, false);
            var result = await _schemes.FindAsync(filter);
            var schemes = await result.ToListAsync();

            return schemes;
        }

        public async Task RemoveSchemeAsync(Guid schemeId)
        {
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);
            var update = Builders<Scheme>.Update.Set(x => x.Removed, true);
            
            await _schemes.UpdateOneAsync(filter, update);
        }
    }
}