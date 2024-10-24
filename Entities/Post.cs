namespace Entities;

public class Post : IEntity {
    
    public int Id { get; set; }
    public string Title { get; set; } 
    public string Body { get; set; } 
    public int UserId { get; set; }

    public Post(int id, string title, string body, int userId)
    {
        this.Id = id;
        this.Title = title;
        this.Body = body;
        this.UserId = userId;
    }
    public Post(string title, string body, int userId)
    {
        this.Title = title;
        this.Body = body;
        this.UserId = userId;
    }
    
    public override string ToString() {
        return $"Post {Id}: {Title}, {Body}, UserId: {UserId}";
    }
}