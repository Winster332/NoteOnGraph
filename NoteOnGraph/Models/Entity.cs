using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoteOnGraph.Models
{
    public class Entity
    {
        [BsonId]
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public bool Removed { get; set; }

        public Entity()
        {
            Id = Guid.Empty;
            Version = 0;
            CreatedDateTime = DateTime.Now;
            ChangeDateTime = DateTime.Now;
            Removed = false;
        }

        protected void Upgrade()
        {
            Version++;
            ChangeDateTime = DateTime.Now;
        }
    }
}