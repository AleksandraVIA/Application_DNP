using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository) {
    public async Task CreateAsync() {
        Console.WriteLine("Enter user name:");
        var name = Console.ReadLine();
        Console.WriteLine("Enter user password:");
        var password = Console.ReadLine();
        var user = new User(userRepository.GetMany().Count() + 1, name, password);
        await userRepository.AddAsync(user);
        Console.WriteLine("User created successfully");
    }
}