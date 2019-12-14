using System;
using System.Collections.Generic;
using NoteOnGraph.Infrastructure;

namespace NoteOnGraph.Models
{
    public class Node : IDbEntity
    {
        public Guid Id { get; set; }
        public Guid DataId { get; set; }
        public Guid SchemeId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public string Title { get; set; }
        public List<Guid> Inputs { get; set; }
        public List<Guid> Outputs { get; set; }
    }
}