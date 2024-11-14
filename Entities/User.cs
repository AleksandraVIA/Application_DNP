namespace Entities;

public class User : IEntity 
{
   
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    
    public User(string username, string password) 
    {
        Username = username;
        Password = password;
    }

    public User(int id, string username, string password)
    {
        Id = id;
        Username = username;
        Password = password;
        
    }

    public override string ToString() 
    {
        return $"User {Id}: {Username}, {Password}";
    }
}