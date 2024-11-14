using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task<AddUserResponseDto> GetUserAsync(int id);
    public Task<List<string>> GetAllUsersAsync(string? nameContains);
    public Task<IResult> DeleteUserAsync(DeleteUserDto reques, int id);
}