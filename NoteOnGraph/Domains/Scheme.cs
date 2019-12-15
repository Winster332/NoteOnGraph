using System;
using System.Collections.Generic;

namespace NoteOnGraph.Domains
{
    public class Scheme : Entity
    {
        public IList<Guid> JointsIds { get; set; }
        public IList<Guid> NodesIds { get; set; }
    }
}