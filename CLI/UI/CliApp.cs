using RepositoryContracts;

namespace CLI.UI;

public class CliApp(
    IUserRepository userRepository,
    ICommentRepository commentRepository,
    IPostRepository postRepository) {
    public async Task StartAsync() {
        while (true) {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Manage users");
            Console.WriteLine("2. Manage posts");
            Console.WriteLine("3. Exit");
            var option = Console.ReadLine();
            switch (option) {
                case "1":
                    await ManageUsersAsync();
                    break;
                case "2":
                    await ManagePostsAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
    
    private async Task ManageUsersAsync() {
        while (true) {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. List users");
            Console.WriteLine("3. Back");
            var option = Console.ReadLine();
            switch (option) {
                case "1":
                    var createUserView = new ManageUsers.CreateUserView(userRepository);
                    await createUserView.CreateAsync();
                    break;
                case "2":
                    var listUsersView = new ManageUsers.ListUsersView(userRepository);
                    await listUsersView.DisplayAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
    
    private async Task ManagePostsAsync() {
        while (true) {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. List posts");
            Console.WriteLine("2. Add comment to post");
            Console.WriteLine("3. View single post");
            Console.WriteLine("4. Back");
            var option = Console.ReadLine();
            switch (option) {
                case "1":
                    var listPostsView = new ManagePosts.ListPostsView(postRepository);
                    await listPostsView.ListAsync();
                    break;
                case "2":
                    var addCommentView = new ManagePosts.AddCommentView(userRepository, postRepository, commentRepository);
                    await addCommentView.AddAsync();
                    break;
                case "3":
                    var viewSinglePostView = new ManagePosts.SinglePostView(postRepository);
                    await viewSinglePostView.ShowAsync();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}