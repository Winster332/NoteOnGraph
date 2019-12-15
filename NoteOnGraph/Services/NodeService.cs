using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using NoteOnGraph.Domains.Aggregators;

namespace NoteOnGraph.Services
{
    public interface INodeService
    {
        Task<Node> Create(float x, float y, string caption);
        Task ChangeCaptionAsync(Guid nodeId, string caption);
        Task ChangePositionXAsync(Guid nodeId, float x);
        Task ChangePositionYAsync(Guid nodeId, float y);
        Task ChangePositionAsync(Guid nodeId, float x, float y);
        Task AddOutputAsync(Guid nodeId, Guid jointId);
        Task RemoveOutputAsync(Guid nodeId, Guid jointId);
        Task AddInputAsync(Guid nodeId, Guid jointId);
        Task<Node> GetNodeAsync(Guid nodeId);
        Task RemoveInputAsync(Guid nodeId, Guid jointId);
        Task RemoveAsync(Guid nodeId);
    }

    public class NodeService : INodeService
    {
        private IMongoCollection<Node> _nodes;
        
        public NodeService(IMongoDatabase database)
        {
            _nodes = database.GetCollection<Node>("nodes");
        }
        
        public async Task<Node> Create(float x, float y, string caption)
        {
            var node = new Node
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Removed = false,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,

                X = x,
                Y = y,
                Caption = caption,
                OutputsIds = new List<Guid>(),
                InputsIds = new List<Guid>(),

            };
            
            await _nodes.InsertOneAsync(node);

            return node;
        }
        
        public async Task ChangeCaptionAsync(Guid nodeId, string caption)
        {
            var update = Builders<Node>.Update
                .Set(x => x.Caption, caption)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, nodeId);
            
            await _nodes.UpdateOneAsync(filter, update);
        }
        
        public async Task ChangePositionXAsync(Guid nodeId, float x)
        {
            var update = Builders<Node>.Update
                .Set(x => x.X, x)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, nodeId);
            
            await _nodes.UpdateOneAsync(filter, update);
        }
        
        public async Task ChangePositionYAsync(Guid nodeId, float y)
        {
            var update = Builders<Node>.Update
                .Set(x => x.Y, y)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, nodeId);
            
            await _nodes.UpdateOneAsync(filter, update);
        }
        
        public async Task ChangePositionAsync(Guid nodeId, float x, float y)
        {
            var update = Builders<Node>.Update
                .Set(x => x.X, x)
                .Set(x => x.Y, y)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, nodeId);
            
            await _nodes.UpdateOneAsync(filter, update);
        }
        
        public async Task AddOutputAsync(Guid nodeId, Guid jointId)
        {
            var node = await GetNodeAsync(nodeId);
            
            node.OutputsIds.Add(jointId);
            
            var update = Builders<Node>.Update
                .Set(x => x.OutputsIds, node.OutputsIds)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, node.Id);
            
            await _nodes.UpdateOneAsync(filter, update);
        }
        
        public async Task RemoveOutputAsync(Guid nodeId, Guid jointId)
        {
            var node = await GetNodeAsync(nodeId);
            
            node.OutputsIds.Remove(jointId);
            
            var update = Builders<Node>.Update
                .Set(x => x.OutputsIds, node.OutputsIds)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, node.Id);
            
            await _nodes.UpdateOneAsync(filter, update);
        }

        public async Task AddInputAsync(Guid nodeId, Guid jointId)
        {
            var node = await GetNodeAsync(nodeId);
            
            node.InputsIds.Add(jointId);
            
            var update = Builders<Node>.Update
                .Set(x => x.InputsIds, node.InputsIds)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, node.Id);
            
            await _nodes.UpdateOneAsync(filter, update);
        }

        public async Task<Node> GetNodeAsync(Guid nodeId)
        {
            var filter = Builders<Node>.Filter.Eq(x => x.Id, nodeId);

            var result = await _nodes.FindAsync(filter);
            var node = result.FirstOrDefault();

            return node;
        }
        
        public async Task RemoveInputAsync(Guid nodeId, Guid jointId)
        {
            var node = await GetNodeAsync(nodeId);
            
            node.InputsIds.Remove(jointId);
            
            var updateX = Builders<Node>.Update
                .Set(x => x.InputsIds, node.InputsIds)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filterX = Builders<Node>.Filter.Eq(x => x.Id, jointId);
            
            await _nodes.UpdateOneAsync(filterX, updateX);
        }
        
        public async Task RemoveAsync(Guid nodeId)
        {
            var update = Builders<Node>.Update
                .Set(x => x.Removed, true)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Node>.Filter.Eq(x => x.Id, nodeId);
            
            await _nodes.UpdateOneAsync(filter, update);
        }
    }
}