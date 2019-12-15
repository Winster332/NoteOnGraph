using System;
using System.Collections.Generic;

namespace NoteOnGraph.Domains
{
    public class Node : Entity
    {
        public string Caption { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public IList<Guid> OutputsIds { get; set; }
        public IList<Guid> InputsIds { get; set; }
    }
}