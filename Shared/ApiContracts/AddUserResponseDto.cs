namespace ApiContracts;

public class AddUserResponseDto
{
    public string Username { get; set; }
    public int UserId { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}