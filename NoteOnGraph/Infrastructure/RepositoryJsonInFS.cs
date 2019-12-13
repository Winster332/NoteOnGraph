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

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        private string ReadFromFile(string pathToFile)
        {
            var fileSource = string.Empty;

            using (var stream = new FileStream(pathToFile, FileMode.Open))
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
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
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

        private string GetPathCollection<T>()
        {
            var type = typeof(T);
            var folder = $"{_filePath}\\{type.Name}";

            return folder;
        }

        private string GetPathCollection<T>(Guid id)
        {
            var folder = GetPathCollection<T>();

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

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
            var path = GetPathCollection<T>(id);
            var fileSource = ReadFromFile(path);

            if (string.IsNullOrEmpty(fileSource))
            {
                return default(T);
            }

            var value = JsonConvert.DeserializeObject<T>(fileSource);

            return value;
        }

        public void Update<T>(T value) where T : IDbEntity
        {
            Create<T>(value);
        }

        public void Delete<T>(Guid id) where T : IDbEntity
        {
            var path = GetPathCollection<T>(id);
            
            File.Delete(path);
        }

        public List<T> GetAll<T>() where T : IDbEntity
        {
            var folder = GetPathCollection<T>();
            var files = Directory.GetFiles(folder);
            var result = new List<T>();
            
            for (var i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                var fileId = Guid.Parse(filePath.Replace(".json", ""));

                var value = Read<T>(fileId);
                result.Add(value);
            }

            return result;
        }

        public void Clear<T>() where T : IDbEntity
        {
            Directory.Delete(_filePath);
        }
    }
}