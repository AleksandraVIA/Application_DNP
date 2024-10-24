
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
    
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        User existingUser = await userRepo.GetSingleAsync(id);
        if (existingUser == null) return NotFound();

        existingUser.Username = request.Username;
        existingUser.Password = request.Password;
        await userRepo.UpdateAsync(existingUser);

        UserDto dto = new UserDto
        {
            Id = existingUser.Id,
            Username = existingUser.Username
        };
        return Ok(dto);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        User user = await userRepo.GetSingleAsync(id);
        if (user == null) return NotFound();

        UserDto dto = new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };
        return Ok(dto);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string search = "")
    {
        IEnumerable<User> users = userRepo.GetMany().ToList(); 

        List<UserDto> userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Username = user.Username
            });
        }

        return Ok(userDtos);
    }

}