public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; } 
    public string Author { get; set; }
    public List<CommentDto> Comments { get; set; } 
}