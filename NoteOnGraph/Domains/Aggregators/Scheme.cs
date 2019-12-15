using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NoteOnGraph.Models;

namespace NoteOnGraph.Domains.Aggregators
{
    public class Scheme : Entity
    {
        public IList<Guid> JointsIds { get; set; }
        public IList<Guid> NodesIds { get; set; }
        
        
        
        public async Task RemoveNode(Guid nodeId)
        {
            Upgrade();

            NodesIds.Remove(nodeId);
            
            var update = Builders<Scheme>.Update.Set(x => x.NodesIds, NodesIds);
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, Id);
            
            await _schemes.UpdateOneAsync(filter, update);
        }
        
        public async Task AddJoint(Guid jointId)
        {
            Upgrade();

            JointsIds.Add(jointId);
            
            var update = Builders<Scheme>.Update.Set(x => x.JointsIds, JointsIds);
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, Id);
            
            await _schemes.UpdateOneAsync(filter, update);
        }
        
        public async Task RemoveJoint(Guid jointId)
        {
            Upgrade();

            JointsIds.Remove(jointId);
            
            var update = Builders<Scheme>.Update.Set(x => x.JointsIds, JointsIds);
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, Id);
            
            await _schemes.UpdateOneAsync(filter, update);
        }
        
        public async Task Remove()
        {
            Upgrade();

            Removed = true;
            
            var update = Builders<Scheme>.Update.Set(x => x.Removed, Removed);
            var filter = Builders<Scheme>.Filter.Eq(x => x.Id, Id);
            
            await _schemes.UpdateOneAsync(filter, update);
        }
    }
}