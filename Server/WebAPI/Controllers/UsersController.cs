
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;


[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    { 
        User user = new User(request.UserName, request.Password);
        User created = await userRepo.AddAsync(user);
        UserDto dto = new UserDto
        {
            Id = created.Id,
            Username = created.Username
        };

       
        return Created($"/Users/{dto.Id}", dto);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? username) {
        IEnumerable<User> users = userRepo.GetMany();
        
        if (username != null) {
            users = users.Where(user => user.Username.Contains(username));
        }
        IEnumerable<UserDto> userDtos = users.Select(user => new UserDto {
            Id = user.Id,
            Username = user.Username
        });
        return Ok(userDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id) {
        User user = await userRepo.GetSingleAsync(id);
        UserDto userDto = new() {
            Id = user.Id,
            Username = user.Username
        };
        return Ok(userDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id) {
        await userRepo.DeleteAsync(id);
        return NoContent();
    }
}