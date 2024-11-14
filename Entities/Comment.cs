namespace Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        
        public override string ToString()
        {
            return $"Comment {Id}: {Body}, PostId: {PostId}, UserId: {UserId}";
        }
    }
}