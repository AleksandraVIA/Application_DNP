using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class AddCommentView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository) {
    public async Task AddAsync() {
        Console.WriteLine("Enter comment content:");
        var content = Console.ReadLine();

        Console.WriteLine("Enter comment author ID:");
        var userId = int.Parse(Console.ReadLine());
        var author = await userRepository.GetSingleAsync(userId);
        if (author == null) {
            Console.WriteLine("Author not found.");
            return;
        }

        Console.WriteLine("Enter post ID:");
        var postId = int.Parse(Console.ReadLine());

        var post = await postRepository.GetSingleAsync(postId);
        if (post == null) {
            Console.WriteLine("Post not found.");
            return;
        }

        var comment = new Comment();
        await commentRepository.AddAsync(comment);

        Console.WriteLine("Comment created successfully");
    }
    
}