using Entities;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class UserInMemoryRepository : BaseInMemoryRepository<User>,
    IUserRepository {
    public UserInMemoryRepository() {
        items = new List<User> {
            new User(1, "User 1", "11111111"),
            new User(2, "User 2", "22222222"),
            new User(3, "User 3", "33333333"),
            new User(4, "User 4", "44444444"),
            new User(5, "User 5", "55555555")
        };
    }

    public override async Task<User> AddAsync(User user) {
        ValidateUser(user);
        return await base.AddAsync(user);
    }

    public override async Task UpdateAsync(User user) {
        ValidateUser(user);
        await base.UpdateAsync(user);
    }

    private void ValidateUser(User user) {
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