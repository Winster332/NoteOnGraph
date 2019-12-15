using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using NoteOnGraph.Domains.Aggregators;
using Serilog;

namespace NoteOnGraph.Services
{
    public interface ISchemeService
    {
        Task<Scheme> CreateSchemeAsync();
        Task AddNodeOnSchemeAsync(Guid schemeId, Guid nodeId);
        Task<Node> CreateNodeOnScheme(Guid schemeId, float x, float y, string caption);
        Task<bool> RemoveNodeFromScheme(Guid schemeId, Guid nodeId);
        Task<Joint> CreateJointOnSchemeAsync(Guid schemeId, Guid nodeFromId, Guid nodeToId);
        Task<bool> RemoveJointOnSchemeAsync(Guid schemeId, Guid jointId);
        Task<Scheme> GetSchemeAsync(Guid schemeId);
        Task<IList<Scheme>> GetSchemesAsync();
        Task RemoveSchemeAsync(Guid schemeId);
    }

    public class SchemeService : ISchemeService
    {
        private IMongoCollection<Scheme> _schemes;
        private IJointService _jointService;
        private INodeService _nodeService;
        
        public SchemeService(IMongoDatabase database, INodeService nodeService, IJointService jointService)
        {
            _schemes = database.GetCollection<Scheme>("schemes");
            _nodeService = nodeService;
            _jointService = jointService;
        }
        
        public async Task<Scheme> CreateSchemeAsync()
        {
            var scheme = new Scheme
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Removed = false,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,

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
                .Set(x => x.ChangedDateTime, DateTime.Now);
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
                .Set(x => x.ChangedDateTime, DateTime.Now);

            await _schemes.UpdateOneAsync(filter, update);

            return node;
        }
        
        public async Task<bool> RemoveNodeFromScheme(Guid schemeId, Guid nodeId)
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
                    .Set(x => x.ChangedDateTime, DateTime.Now);

                await _schemes.UpdateOneAsync(filterScheme, update);

//                for (var i = 0; i < scheme.NodesIds.Count; i++)
//                {
//                    var currentNodeId = scheme.NodesIds[i];
//
//                    var node = await _nodeService.GetNodeAsync(currentNodeId);
//                    
//                    for (var j = 0; j < node.InputsIds.Count; j++)
//                    {
//                        var inputId = node.InputsIds[j];
//
//                        await _jointService.RemoveAsync(inputId);
//                    }
//                    
//                    for (var j = 0; j < node.OutputsIds.Count; j++)
//                    {
//                        var outputId = node.OutputsIds[j];
//
//                        await _jointService.RemoveAsync(outputId);
//                    }
//                }

                isRemoved = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

                isRemoved = false;
            }

            return isRemoved;
        }
        
        public async Task<Joint> CreateJointOnSchemeAsync(Guid schemeId, Guid nodeFromId, Guid nodeToId)
        {
            var joint = await _jointService.CreateAsync(nodeFromId, nodeToId);
            
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);

            var schemes = await _schemes.FindAsync(filter);
            var scheme = schemes.FirstOrDefault();

            scheme.JointsIds.Add(joint.Id);

            var update = Builders<Scheme>.Update
                .Set(x => x.JointsIds, scheme.JointsIds)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            
            await _schemes.UpdateOneAsync(filter, update);

            return joint;
        }
        
        public async Task<bool> RemoveJointOnSchemeAsync(Guid schemeId, Guid jointId)
        {
            var isRemoved = false;

            try
            {
                await _jointService.RemoveAsync(jointId);

                var filterScheme = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);

                var schemes = await _schemes.FindAsync(filterScheme);
                var scheme = schemes.FirstOrDefault();

                scheme.JointsIds.Remove(jointId);

                var filter = Builders<Scheme>.Filter.Eq(x => x.Id, schemeId);
                var update = Builders<Scheme>.Update
                    .Set(x => x.JointsIds, scheme.JointsIds)
                    .Set(x => x.ChangedDateTime, DateTime.Now);

                await _schemes.UpdateOneAsync(filter, update);
                
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