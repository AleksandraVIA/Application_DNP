using System.Formats.Tar;
using System.Globalization;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts;
    public Task<Post> AddAsync(Post post)
    {
        post.id = posts.Any()
            ? posts.Max(p => p.id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post ? existingPost = posts.SingleOrDefault(p => p.id == post.id);
        if (existingPost is null)
        {
            throw new InvalidOperationException( $"Post with ID {post.id} not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post ? postToRemove = posts.SingleOrDefault(p => p.id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID {id} not found");
        }
            posts.Remove(postToRemove);
            return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post ? postToGet = posts.SingleOrDefault(p => p.id == id);
        if (postToGet is null)
        {
            throw new InvalidOperationException($"Post with ID {id} not found");
        }
        postToGet.id = id;
        return Task.FromResult(postToGet);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}