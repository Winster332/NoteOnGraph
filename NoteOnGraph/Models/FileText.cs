using System;
using NoteOnGraph.Infrastructure;

namespace NoteOnGraph.Models
{
    public class FileText : IDbEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}