using System;
using System.Collections.Generic;

namespace NoteOnGraph.Infrastructure
{
    public interface IRepository
    {
        void Create<T>(T value) where T : IDbEntity;
        T Read<T>(Guid id) where T : IDbEntity;
        void Update<T>(T value) where T : IDbEntity;
        void Delete<T>(Guid id) where T : IDbEntity;
        List<T> GetAll<T>() where T : IDbEntity;
        void Clear<T>() where T : IDbEntity;
    }
}