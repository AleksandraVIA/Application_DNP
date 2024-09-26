using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : BaseFileRepository<Post>, IPostRepository {
    public PostFileRepository() : base("posts.json") {
        if (!File.Exists("posts.json")) {
            var dummyPosts = new List<Post> {
                new Post ( 1, "First Post", "This is the first post." , 1),
                new Post ( 2, "Second Post", "This is the second post." , 2)
            };
            string postsJson = JsonSerializer.Serialize(dummyPosts);
            File.WriteAllText("posts.json", postsJson);
        }
    }
}