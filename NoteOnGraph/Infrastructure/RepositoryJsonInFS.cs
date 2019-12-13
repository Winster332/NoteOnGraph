using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace NoteOnGraph.Infrastructure
{
    public class RepositoryJsonInFS : IRepository
    {
        private string _filePath;
        
        public RepositoryJsonInFS(string filePath)
        {
            _filePath = filePath;
        }

        private string ReadFromFile(string pathToFile)
        {
            var fileSource = string.Empty;
            
            using (var stream = new FileStream($"{_filePath}\\{pathToFile}", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    fileSource = reader.ReadToEnd();
                }
            }

            return fileSource;
        }

        private bool WriteToFile(string fileSource, string filePath)
        {
            try
            {
                using (var stream = new FileStream($"{_filePath}\\{filePath}", FileMode.OpenOrCreate))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(fileSource);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

                return false;
            }

            return true;
        }

        private string GetPathCollection<T>(Guid id)
        {
            var type = typeof(T);
            var folder = type.Name;
            var file = $"{id}.json";
            var path = $"{folder}\\{file}";

            return path;
        }
        
        public void Create<T>(T value) where T : IDbEntity
        {
            var jsonString = JsonConvert.SerializeObject(value);
            var path = GetPathCollection<T>(value.Id);

            if (WriteToFile(jsonString, path))
            {
            }
        }

        public T Read<T>(Guid id) where T : IDbEntity
        {
            
        }

        public void Update<T>(T value) where T : IDbEntity
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(Guid id) where T : IDbEntity
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public void Clear<T>() where T : IDbEntity
        {
            throw new NotImplementedException();
        }
    }
}