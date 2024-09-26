using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView (IPostRepository postRepository) {
    public async Task ShowAsync() {
        Console.WriteLine("Enter post ID:");
        var id = int.Parse(Console.ReadLine());

        var post = await postRepository.GetSingleAsync(id);
        if (post == null) {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.WriteLine($"ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Content: {post.Body}");
        Console.WriteLine($"Author ID: {post.UserId}");
    }
    
}