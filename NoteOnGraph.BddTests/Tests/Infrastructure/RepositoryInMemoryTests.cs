using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NoteOnGraph.Infrastructure;
using NoteOnGraph.Models;
using Xunit;

namespace NoteOnGraph.BddTests.Tests.Infrastructure
{
    public class RepositoryInMemoryTests
    {
        public IRepository Repository { get; set; }
        
        public RepositoryInMemoryTests()
        {
            Repository = new RepositoryInMemory();
        }

        [Fact]
        public void RepositoryInMemory_CreateInstance_ReturnResult()
        {
            var file = new File
            {
                Id = Guid.NewGuid(),
                Href = "localhost",
                Title = "test",
                Type = BlobType.File
            };
            
            Repository.Create<File>(file);

            var assertionsFiles = Repository.GetAll<File>();
            assertionsFiles.Should().HaveCount(1);
            
            var assertionFile = assertionsFiles.FirstOrDefault();
            assertionFile.Should().NotBeNull();
            
            assertionFile.Should().BeEquivalentTo(file);
        }

        [Fact]
        public void RepositoryInMemory_RemoveInstance_ReturnSuccess()
        {
            for (var i = 0; i < 10; i++)
            {
                var file = new File
                {
                    Id = Guid.NewGuid(),
                    Href = "localhost",
                    Title = $"test-{i}",
                    Type = BlobType.File
                };

                Repository.Create<File>(file);
            }
            
            var assertionsFiles = Repository.GetAll<File>();
            assertionsFiles.Should().HaveCount(10);
            
            var assertionFile = assertionsFiles.FirstOrDefault();
            assertionFile.Should().NotBeNull();
            
            Repository.Delete<File>(assertionFile.Id);
            
            var newFiles = Repository.GetAll<File>();
            newFiles.Should().HaveCount(9);

            newFiles.Contains(assertionFile).Should().BeFalse();
        }

        [Fact]
        public void RepositoryInMemory_ClearData_ReturnSuccess()
        {
            for (var i = 0; i < 10; i++)
            {
                var file = new File
                {
                    Id = Guid.NewGuid(),
                    Href = "localhost",
                    Title = $"test-{i}",
                    Type = BlobType.File
                };

                Repository.Create<File>(file);
            }
            
            var assertionsFiles = Repository.GetAll<File>();
            assertionsFiles.Should().HaveCount(10);
            
            Repository.Clear<File>();
            
            var newFiles = Repository.GetAll<File>();
            newFiles.Should().HaveCount(0);
        }

        [Fact]
        public void RepositoryInMemory_ReadFromRepository_ReturnFile()
        {
            var files = new List<File>();
            
            for (var i = 0; i < 10; i++)
            {
                var file = new File
                {
                    Id = Guid.NewGuid(),
                    Href = "localhost",
                    Title = $"test-{i}",
                    Type = BlobType.File
                };
                files.Add(file);

                Repository.Create<File>(file);
            }

            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];

                var assertionFile = Repository.Read<File>(file.Id);
                
                assertionFile.Should().BeEquivalentTo(file);
            }
        }
        
        [Fact]
        public void RepositoryInMemory_UpdateFile_ReturnSuccess()
        {
            var files = new List<File>();
            
            for (var i = 0; i < 10; i++)
            {
                var file = new File
                {
                    Id = Guid.NewGuid(),
                    Href = "localhost",
                    Title = $"test-{i}",
                    Type = BlobType.File
                };
                files.Add(file);

                Repository.Create<File>(file);

                file.Title = $"OK-{i}";
                Repository.Update(file);
            }

            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];

                var assertionFile = Repository.Read<File>(file.Id);

                assertionFile.Title.Should().BeEquivalentTo($"OK-{i}");
            }
        }
    }
}