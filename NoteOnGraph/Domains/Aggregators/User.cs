using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace NoteOnGraph.Domains.Aggregators
{
    public class User : Entity
    {
        public string AvatarRef { get; private set; }
        public string Bio { get; private set; }
        public string Email { get; set; }
        
        public string Login { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        
        public IList<Guid> SchemesIds { get; set; }
        
        private IMongoCollection<User> _users;
        
        public User(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task Create(string login, string firstName, string lastName, string avatarRef, string bio)
        {
            Id = Guid.NewGuid();
            Version = 1;
            Removed = false;
            CreatedDateTime = DateTime.Now;
            ChangedDateTime = DateTime.Now;
            
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            AvatarRef = avatarRef;
            Bio = bio;
            SchemesIds = new List<Guid>();
            
            await _users.InsertOneAsync(this);
        }

        public async Task ChangeFirstName(string firstName)
        {

            FirstName = firstName;
            
            var update = Builders<User>.Update.Set(x => x.FirstName, FirstName);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }

        public async Task ChangeAvatarReference(string avatarReference)
        {

            AvatarRef = avatarReference;
            
            var update = Builders<User>.Update.Set(x => x.AvatarRef, AvatarRef);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }
        
        public async Task ChangeBio(string bio)
        {

            Bio = bio;
            
            var update = Builders<User>.Update.Set(x => x.Bio, Bio);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }
        
        public async Task ChangeLogin(string login)
        {

            Login = login;
            
            var update = Builders<User>.Update.Set(x => x.Login, Login);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }
        
        public async Task ChangeLastName(string lastName)
        {

            LastName = lastName;
            
            var update = Builders<User>.Update.Set(x => x.LastName, lastName);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }

        public async Task ChangeEmail(string email)
        {

            Email = email;
            
            var update = Builders<User>.Update.Set(x => x.Email, Email);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }

        public async Task AddScheme(Guid schemeId)
        {

            SchemesIds.Add(schemeId);
            
            var update = Builders<User>.Update.Set(x => x.SchemesIds, SchemesIds);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }
        
        public async Task RemoveScheme(Guid schemeId)
        {

            SchemesIds.Remove(schemeId);
            
            var update = Builders<User>.Update.Set(x => x.SchemesIds, SchemesIds);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }
        
        public async Task Remove()
        {

            Removed = true;
            
            var update = Builders<User>.Update.Set(x => x.Removed, Removed);
            var filter = Builders<User>.Filter.Eq(x => x.Id, Id);
            
            await _users.UpdateOneAsync(filter, update);
        }
    }
}