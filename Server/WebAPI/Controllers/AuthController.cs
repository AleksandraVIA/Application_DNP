using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AddUserResponseDto>> Login([FromBody] LoginRequest request)
    {
        User? user = userRepository.GetMany().AsEnumerable().SingleOrDefault(u => u.Username == request.Email);
        
        if (user == null)
            return Unauthorized("Username is incorrect");

        if (!user.Password.Equals(request.Password))
            return Unauthorized("Password is incorrect");

        var sendDto = new AddUserResponseDto { Username = user.Username, UserId = user.Id };
        return sendDto;
    }


}