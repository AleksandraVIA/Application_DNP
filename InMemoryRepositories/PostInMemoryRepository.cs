using Entities;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class PostInMemoryRepository(IUserRepository userRepository) : BaseInMemoryRepository<Post>,
    IPostRepository
{
    public override async Task<Post> AddAsync(Post post) {
        ValidatePost(post);
        return await base.AddAsync(post);
    }

    public override async Task UpdateAsync(Post post) {
        ValidatePost(post);
        await base.UpdateAsync(post);
    }

    private void ValidatePost(Post post) {
        if (string.IsNullOrWhiteSpace(post.Title)) {
            throw new InvalidOperationException("Post title is required");
        }

        if (string.IsNullOrWhiteSpace(post.Body)) {
            throw new InvalidOperationException("Post body is required");
        }

        if (items.Any(p => p.PostId == post.PostId)) {
            throw new InvalidOperationException(
                "Post with the same id already exists");
        }

        if (!userRepository.GetMany().Any(u => u.Id == post.UserId)) {
            throw new InvalidOperationException(
                "Post must be made by an existing user");
        }
    }
}