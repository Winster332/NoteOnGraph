using System;
using System.Collections.Generic;
using NoteOnGraph.Infrastructure;

namespace NoteOnGraph.Models
{
    public enum BlobType
    {
        File,
        Folder
    }

    public class Project : IDbEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public BlobType Type { get; set; }
        public IList<Guid> Files { get; set; }
    }
}