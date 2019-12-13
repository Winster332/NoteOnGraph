using System;
using System.Collections.Generic;
using System.Linq;

namespace NoteOnGraph.Infrastructure
{
    public class RepositoryInMemory : IRepository
    {
        private Dictionary<Type, Dictionary<Guid, IDbEntity>> _data;

        public RepositoryInMemory()
        {
            _data = new Dictionary<Type, Dictionary<Guid, IDbEntity>>();
        }
        
        public void Create<T>(T value) where T : IDbEntity
        {
            var collectionType = typeof(T);

            if (!_data.ContainsKey(collectionType))
            {
                _data.Add(typeof(T), new Dictionary<Guid, IDbEntity>());
            }

            var collection = _data[collectionType];

            if (!collection.ContainsKey(value.Id))
            {
                collection.Add(value.Id, value);
            }
        }

        public T Read<T>(Guid id) where T : IDbEntity
        {
            var entity = ValidationData<T>(id);

            return entity;
        }

        public void Update<T>(T value) where T : IDbEntity
        {
            var collectionType = typeof(T);

            if (!_data.ContainsKey(collectionType))
            {
                return;
            }

            var collection = _data[collectionType];

            if (!collection.ContainsKey(value.Id))
            {
                return;
            }

            collection[value.Id] = value;
        }

        public void Delete<T>(Guid id) where T : IDbEntity
        {
            var data = ValidationData<T>(id);

            if (data == null)
            {
                return;
            }

            _data[typeof(T)].Remove(data.Id);
        }

        public List<T> GetAll<T>()
        {
            var collectionType = typeof(T);
            
            if (!_data.ContainsKey(collectionType))
            {
                return new List<T>();
            }
            
            return _data[collectionType].OfType<T>().ToList();
        }

        public void Clear<T>() where T : IDbEntity
        {
            var collectionType = typeof(T);

            if (!_data.ContainsKey(collectionType))
            {
                return;
            }
            
            _data[collectionType].Clear();
        }

        private T ValidationData<T>(Guid id)
        {
            var collectionType = typeof(T);

            if (!_data.ContainsKey(collectionType))
            {
                return default(T);
            }

            var collection = _data[collectionType];

            if (!collection.ContainsKey(id))
            {
                return default(T);
            }

            return (T) collection[id];
        }
    }
}