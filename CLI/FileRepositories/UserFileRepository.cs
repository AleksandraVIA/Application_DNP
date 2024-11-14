using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            AddAsync();
        }
    }

    public async Task AddAsync()
    {
        int delay = 0;
        Thread.Sleep(delay);
        await AddAsync(new User
            { Username = "bitty", Password = "1234" });
        Thread.Sleep(delay);
        await AddAsync(new User
            { Username = "bella", Password = "12345" });
        Thread.Sleep(delay);
        await AddAsync(new User
            { Username = "bob123", Password = "123456" });
        Thread.Sleep(delay);
        await AddAsync(new User
            { Username = "bob", Password = "11111" });
        Thread.Sleep(delay);
        await AddAsync(new User
            { Username = "boss", Password = "qwerty" });
    }

    public async Task<User?> AddAsync(User? user)
    {
        List<User?> users = await LoadUsersAsync();
        int maxId = users.Count > 0 ? users.Max(like => like.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);
        SaveUsersAsync(users);
        return user;
    }

    public async Task UpdateAsync(User? user)
    {
        List<User?> users = await LoadUsersAsync();

        User? userToUpdate = await GetSingleAsync(user.Id);

        users.Remove(userToUpdate);
        users.Add(user);
        SaveUsersAsync(users);
    }

    public async Task DeleteAsync(int userId)
    {
        List<User?> users = await LoadUsersAsync();
        User? userToDelete = await GetSingleAsync(userId);
        users.Remove(userToDelete);
        SaveUsersAsync(users);
    }

    public async Task<User?> GetSingleAsync (int userId)
    {
        List<User?> users = await LoadUsersAsync();
        User? user = users.SingleOrDefault(l => l.Id == userId);
        if (user is null)
            throw new InvalidOperationException(
                $"User with ID {userId} not found.");
        return user;
    }

    public IQueryable<User?> GetMany()
    {
        List<User?> users = LoadUsersAsync().Result;
        return users.AsQueryable();
    }
    
    private async Task<List<User?>> LoadUsersAsync()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User?> users =
            JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users;
    }

    private async void SaveUsersAsync(List<User?> toSaveUsers)
    {
        string usersAsJson = JsonSerializer.Serialize(toSaveUsers,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }
}