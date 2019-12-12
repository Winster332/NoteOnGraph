using System;
using System.Collections.Generic;

namespace NoteOnGraph.Models
{
    public enum BlobType
    {
        File,
        Folder
    }

    public class Project
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public BlobType Type { get; set; }
        public List<File> Files { get; set; }
    }
}