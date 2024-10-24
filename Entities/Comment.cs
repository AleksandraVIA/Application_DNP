namespace Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

       
        public Comment(string body, int postId, int userId)
        {
            Body = body;
            PostId = postId;
            UserId = userId;
        }
        
        public Comment(int id, string body, int postId, int userId)
        {
            Id = id;
            Body = body;
            PostId = postId;
            UserId = userId;
        } 

        public override string ToString()
        {
            return $"Comment {Id}: {Body}, PostId: {PostId}, UserId: {UserId}";
        }
    }
}