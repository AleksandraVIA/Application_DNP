using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            AddAsync();
        }
    }

    public async Task AddAsync()
    {
        int delay = 0;
        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "Solar eclipse today.",
            Body = "Don't miss today's solar eclipse at 15:37.", UserId = 1
        });
        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "Thread with dad jokes",
            Body = "Leave your best dad jokes in comments.", UserId = 2
        });

        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "Best supervisor at VIA.",
            Body = "Haha, got your attention.", UserId = 3
        });
        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "Thread with favorite books:", Body = "Lad os begynde",
            UserId = 3
        });

        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "DNP Exam",
            Body =
                "No grade other than 12 is acceptable. Can we build it? Ja, selvfoelgelig!",
            UserId = 4
        });
        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "This is just another post.",
            Body = "It's Sunday, 20:57 and I'm tired. Oh nej.", UserId = 5
        });
        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "Pain... pain everywhere",
            Body =
                "I'm in pain and Panodil doesn't help anymore. What should I do?",
            UserId = 5
        });
        Thread.Sleep(delay);
        await AddAsync(new Post
        {
            Title = "Is the water wet?",
            Body = "Tell me, please tell me. I need answers", UserId = 5
        });
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadAsync();
        int maxId = posts.Count > 0 ? posts.Max(p => p.PostId) : 0;
        post.PostId = maxId + 1;
        posts.Add(post);
        SaveAsync(posts);
        return post;
    }


    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadAsync();
        Post postToUpdate = await GetSingleAsync(post.PostId);
        posts.Remove(postToUpdate);
        posts.Add(post);
        SaveAsync(posts);
    }

    public async Task DeleteAsync(int postId)
    {
        List<Post> posts = await LoadAsync();
        Post postToDelete = await GetSingleAsync(postId);
        posts.Remove(postToDelete);
        SaveAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int postId)
    {
        List<Post> posts = await LoadAsync();
        Post? post = posts.SingleOrDefault(p => p.PostId == postId);
        if (post is null)
            throw new InvalidOperationException($"No post with ID {postId}.");
        return post;
    }

    public IQueryable<Post> GetMany()
    {
        List<Post> posts = LoadAsync().Result;
        return posts.AsQueryable();
    }

    private async Task<List<Post>> LoadAsync()
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts;
    }

    private async void SaveAsync(List<Post> toSavePosts)
    {
        string postsAsJson = JsonSerializer.Serialize(toSavePosts,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }
}