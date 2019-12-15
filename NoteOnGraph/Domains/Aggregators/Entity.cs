using System;

namespace NoteOnGraph.Domains.Aggregators
{
    public class Entity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ChangedDateTime { get; set; }
        public bool Removed { get; set; }
        public int Version { get; set; }

        public Entity()
        {
            Id = Guid.Empty;
            Removed = false;
            Version = 1;
            CreatedDateTime = DateTime.Now;
            ChangedDateTime = DateTime.Now;
        }
    }
}