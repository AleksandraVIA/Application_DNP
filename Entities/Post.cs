namespace Entities;

public class Post : IEntity
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    private string? title, body;

    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The title cannot be empty.");
            title = value;
        }
    }

    public string Body {
        get => body;
        set
        {
            if (value is null || value.Trim().Equals(""))
                throw new ArgumentException("The body cannot be empty.");
            body = value;
        }
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        Post other=(Post)obj;
        if (other.UserId==UserId && other.Title.Equals(Title) && other.Body.Equals(Body) && other.PostId==PostId)
            return true;
        return false;
    }

    public int Id { get; set; }
}