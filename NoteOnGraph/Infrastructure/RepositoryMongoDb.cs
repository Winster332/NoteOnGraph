using MongoDB.Driver;

namespace NoteOnGraph.Infrastructure
{
    public class RepositoryMongoDb
    {
        public IMongoDatabase Database { get; set; }
        public RepositoryMongoDb()
        {
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase("NoteOnGraph");
        }
    }
}