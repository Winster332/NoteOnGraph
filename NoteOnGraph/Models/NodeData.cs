using System;
using NoteOnGraph.Infrastructure;

namespace NoteOnGraph.Models
{
    public class NodeData : IDbEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid NodeId { get; set; }
    }
}