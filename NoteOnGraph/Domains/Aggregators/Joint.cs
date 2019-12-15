using System;

namespace NoteOnGraph.Domains.Aggregators
{
    public class Joint : Entity
    {
        public Guid NodeFromId { get; set; }
        public Guid NodeToId { get; set; }
    }
}