using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : BaseFileRepository<User>, IUserRepository {
    public UserFileRepository() : base("users.json") {
        if (!File.Exists("users.json")) {
            var dummyUsers = new List<User> {
               
            };
            string usersJson = JsonSerializer.Serialize(dummyUsers);
            File.WriteAllText("users.json", usersJson);
        }
    }
}