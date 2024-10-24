using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : BaseFileRepository<Comment>, ICommentRepository {
    public CommentFileRepository() : base("comments.json") {
        if (!File.Exists("comments.json")) {
            var dummyComments = new List<Comment> {
            };
            string commentsJson = JsonSerializer.Serialize(dummyComments);
            File.WriteAllText("comments.json", commentsJson);
        }
    }
}