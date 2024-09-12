using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    
    private List <User>  users;
    
    public Task<User> AddAsync(User user)
    {
        user.id = users.Any() ? users.Max(x => x.id) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }
    
    public Task UpdateAsync(User user)
    {
        User ? existingUser = users.FirstOrDefault(u => u.id == user.id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with id {user.id} not found");
        }

        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User ? userToRemove = users.FirstOrDefault(u => u.id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with id {id} not found");
        }
        users.Remove(userToRemove);
        return Task.CompletedTask;
        
    }

    public Task<User> GetByIdAsync(int id)
    {
        User ? userToGet = users.FirstOrDefault(u => u.id == id);
        if (userToGet is null)
        {
            throw new InvalidOperationException($"User with id {id} not found");
        }

        userToGet.id = id;
        return Task.FromResult(userToGet);
    }

    public IQueryable<User> GetAll()
    {
        return users.AsQueryable();
    }
}