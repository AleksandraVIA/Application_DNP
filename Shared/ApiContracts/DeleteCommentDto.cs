namespace ApiContracts;

public class DeleteCommentDto
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }  
    public string AuthorsName { get; set; }
    public int UserId { get; set; }  

}