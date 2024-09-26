using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView (IPostRepository postRepository) {
    public async Task ListAsync() {
        var posts = postRepository.GetMany();


        foreach (var post in posts) {
            Console.WriteLine($"ID: {post.Id}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Content: {post.Body}");
            Console.WriteLine($"Author ID: {post.UserId}");
            Console.WriteLine();
        }
    }
    
}