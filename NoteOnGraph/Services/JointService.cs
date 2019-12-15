using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using NoteOnGraph.Domains.Aggregators;

namespace NoteOnGraph.Services
{
    public interface IJointService
    {
        Task<Joint> CreateAsync(Guid nodeFromId, Guid nodeToId);
        Task RemoveAsync(Guid jointId);
    }

    public class JointService : IJointService
    {
        private IMongoCollection<Joint> _joints;
        
        public JointService(IMongoDatabase database)
        {
            _joints = database.GetCollection<Joint>("joints");
        }
        
        public async Task<Joint> CreateAsync(Guid nodeFromId, Guid nodeToId)
        {
            var joint = new Joint
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Removed = false,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,

                NodeFromId = nodeFromId,
                NodeToId = nodeToId
            };
            
            await _joints.InsertOneAsync(joint);

            return joint;
        }
        
        public async Task RemoveAsync(Guid jointId)
        {
            var update = Builders<Joint>.Update
                .Set(x => x.Removed, true)
                .Set(x => x.ChangedDateTime, DateTime.Now);
            var filter = Builders<Joint>.Filter.Eq(x => x.Id, jointId);
            
            await _joints.UpdateOneAsync(filter, update);
        }
    }
}