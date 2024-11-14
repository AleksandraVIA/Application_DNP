using Entities;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class UserInMemoryRepository : BaseInMemoryRepository<User>,
    IUserRepository {
    
    public override async Task<User?> AddAsync(User? user) {
        ValidateUser(user);
        return await base.AddAsync(user);
    }

    public override async Task UpdateAsync(User? user) {
        ValidateUser(user);
        await base.UpdateAsync(user);
    }

    private void ValidateUser(User? user) {
        if (string.IsNullOrWhiteSpace(user.Username)) {
            throw new InvalidOperationException("Username is required");
        }

        if (string.IsNullOrWhiteSpace(user.Password)) {
            throw new InvalidOperationException("Password is required");
        }

        if (items.Any(u => u.Id == user.Id)) {
            throw new InvalidOperationException(
                "User with the same id already exists");
        }
    }
    
}