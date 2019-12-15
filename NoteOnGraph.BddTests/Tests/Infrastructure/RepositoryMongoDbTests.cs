using System;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;
using Xunit;
using Node = NoteOnGraph.Domains.Aggregators.Node;

namespace NoteOnGraph.BddTests.Tests.Infrastructure
{
    public class RepositoryMongoDbTests
    {
        [Fact]
        public async Task Te()
        {
            var r = new RepositoryMongoDb();
            var nodes = r.Database.GetCollection<Node>("nodes");
            var c = r.Database.GetCollection<Vec2>("vectors");

            for (var i = 0; i < 10; i++)
            {
                var node = new Node(nodes);
                await node.Create(i, 12, $"x-{i}");
            }

            var result = await nodes.FindAsync(FilterDefinition<Node>.Empty);
            var v = await result.ToListAsync();
            
            Console.WriteLine("123");
        }
    }

    public class Vec2
    {
        [BsonId]
        public Guid Id { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        public Vec2(float x, float y)
        {
            Id = Guid.NewGuid();
            X = x;
            Y = y;
        }
    }
}