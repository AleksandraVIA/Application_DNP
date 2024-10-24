using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : BaseFileRepository<Post>, IPostRepository {
    public PostFileRepository() : base("posts.json") {
        if (!File.Exists("posts.json")) {
            var dummyPosts = new List<Post> {
               
            };
            string postsJson = JsonSerializer.Serialize(dummyPosts);
            File.WriteAllText("posts.json", postsJson);
        }
    }
}