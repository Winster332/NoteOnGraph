using System;
using NoteOnGraph.Infrastructure;

namespace NoteOnGraph.Models
{
    public class Joint : IDbEntity
    {
        public Guid Id { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
    }
}