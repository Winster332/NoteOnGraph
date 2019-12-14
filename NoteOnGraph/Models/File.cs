using System;
using System.Collections.Generic;
using NoteOnGraph.Infrastructure;

namespace NoteOnGraph.Models
{
    public class File : IDbEntity
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public BlobType Type { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public Guid SchemeId { get; set; }
    }
}