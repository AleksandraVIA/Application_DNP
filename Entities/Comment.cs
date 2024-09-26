namespace Entities;

public class Comment(int id, string body, int postId, int userId)
    : IEntity {
    public int Id { get; set; } = id;
    public string Body { get; set; } = body;
    public int PostId { get; set; } = postId;
    public int UserId { get; set; } = userId;
    
    public override string ToString() {
        return $"Comment {Id}: {Body}, PostId: {PostId}, UserId: {UserId}";
    }
}