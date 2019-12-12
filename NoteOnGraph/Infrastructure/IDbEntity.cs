using System;

namespace NoteOnGraph.Infrastructure
{
    public interface IDbEntity
    {
        Guid Id { get; set; }
    }
}