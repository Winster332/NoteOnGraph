using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using NoteOnGraph.Domains.Aggregators;

namespace NoteOnGraph.Repositories
{
    public interface INodeRepository
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

    public class NodeRepository : INodeRepository
    {
        private IMongoCollection<Node> _nodes;
        
        public NodeRepository(IMongoDatabase database)
        {
            _nodes = database.GetCollection<Node>("nodes");
        }
        
        public Task<Node> Create(float x, float y, string caption)
        {
            throw new NotImplementedException();
        }

        public Task ChangeCaptionAsync(Guid nodeId, string caption)
        {
            throw new NotImplementedException();
        }

        public Task ChangePositionXAsync(Guid nodeId, float x)
        {
            throw new NotImplementedException();
        }

        public Task ChangePositionYAsync(Guid nodeId, float y)
        {
            throw new NotImplementedException();
        }

        public Task ChangePositionAsync(Guid nodeId, float x, float y)
        {
            throw new NotImplementedException();
        }

        public Task AddOutputAsync(Guid nodeId, Guid jointId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveOutputAsync(Guid nodeId, Guid jointId)
        {
            throw new NotImplementedException();
        }

        public Task AddInputAsync(Guid nodeId, Guid jointId)
        {
            throw new NotImplementedException();
        }

        public Task<Node> GetNodeAsync(Guid nodeId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveInputAsync(Guid nodeId, Guid jointId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid nodeId)
        {
            throw new NotImplementedException();
        }
    }
}