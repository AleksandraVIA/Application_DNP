using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IUserRepository userRepository, IPostRepository postRepository) {
    public async Task CreateAsync() {
        Console.WriteLine("Enter post title:");
        var title = Console.ReadLine();

        Console.WriteLine("Enter post content:");
        var body = Console.ReadLine();
        Console.WriteLine("Enter post author ID:");
        var userId = int.Parse(Console.ReadLine());
        var author = await userRepository.GetSingleAsync(userId);

        if (author == null) {
            Console.WriteLine("Author not found.");
            return;
        }

        var post = new Post();
        await postRepository.AddAsync(post);

        Console.WriteLine("Post created successfully");
    }
    
}