using System;

namespace NoteOnGraph.WebApi.Models
{
    public class CreateJointModel
    {
        public Guid SchemeId { get; set; }
        public Guid NodeFromId { get; set; }
        public Guid NodeToId { get; set; }
    }
}