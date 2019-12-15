using System;

namespace NoteOnGraph.WebApi.Models
{
    public class CreateNodeModel
    {
        public Guid SchemeId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}