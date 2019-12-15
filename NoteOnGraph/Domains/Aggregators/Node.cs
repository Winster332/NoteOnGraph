using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NoteOnGraph.Models;

namespace NoteOnGraph.Domains.Aggregators
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