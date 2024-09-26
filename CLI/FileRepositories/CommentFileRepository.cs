using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : BaseFileRepository<Comment>, ICommentRepository {
    public CommentFileRepository() : base("comments.json") {
        if (!File.Exists("comments.json")) {
            var dummyComments = new List<Comment> {
                new Comment ( 1, "First Comment", 1 , 1),
                new Comment ( 2, "Second Comment", 2 , 2)
            };
            string commentsJson = JsonSerializer.Serialize(dummyComments);
            File.WriteAllText("comments.json", commentsJson);
        }
    }
}