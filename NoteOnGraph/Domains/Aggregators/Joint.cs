using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NoteOnGraph.Models;

namespace NoteOnGraph.Domains.Aggregators
{
    public class Joint : Entity
    {
        public Guid NodeFromId { get; set; }
        public Guid NodeToId { get; set; }
        
        private IMongoCollection<Joint> _joints;
        
        public Joint(IMongoCollection<Joint> joints)
        {
            _joints = joints;
        }
        
        public async Task Create(Guid nodeFromId, Guid nodeToId)
        {
            Id = Guid.NewGuid();
            Version = 1;
            Removed = false;
            CreatedDateTime = DateTime.Now;
            ChangeDateTime = DateTime.Now;

            NodeFromId = nodeFromId;
            NodeToId = nodeToId;
            
            await _joints.InsertOneAsync(this);
        }
        
        public async Task Remove()
        {
            Upgrade();

            Removed = true;
            
            var update = Builders<Joint>.Update.Set(x => x.Removed, Removed);
            var filter = Builders<Joint>.Filter.Eq(x => x.Id, Id);
            
            await _joints.UpdateOneAsync(filter, update);
        }
    }
}