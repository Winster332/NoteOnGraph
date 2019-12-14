using System;
using System.Collections.Generic;
using NoteOnGraph.Infrastructure;

namespace NoteOnGraph.Models
{
    public class SchemeGraph : IDbEntity
    {
        public Guid Id { get; set; }
        public List<Guid> Nodes { get; set; }
        public List<Guid> Joints { get; set; }
        public Guid FileId { get; set; }
    }
}