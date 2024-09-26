using Entities;
using RepositoryContracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CLI.UI.ManageUsers {
    public class ListUsersView(IUserRepository userRepository) {
        public async Task DisplayAsync() {
            var users = userRepository.GetMany().ToList();

            if (!users.Any()) {
                Console.WriteLine("No users found.");
                return;
            }

            Console.WriteLine("List of users:");
            foreach (var user in users) {
                Console.WriteLine($"ID: {user.Id}, Username: {user.Username}");
            }
        }
    }
}