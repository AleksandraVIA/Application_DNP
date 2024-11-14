using Entities;
using RepositoryContracts;
using System.Collections.Generic;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : BaseInMemoryRepository<Comment>,
    ICommentRepository {
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public CommentInMemoryRepository(IUserRepository userRepository, IPostRepository postRepository) {
        this._userRepository = userRepository;
        this._postRepository = postRepository;
        
    }
    
    public override async Task<Comment> AddAsync(Comment comment) {
        ValidateComment(comment);
        return await base.AddAsync(comment);
    }
    
    public override async Task UpdateAsync(Comment comment) {
        ValidateComment(comment);
        await base.UpdateAsync(comment);
    }

    private void ValidateComment(Comment comment) {
        if (string.IsNullOrWhiteSpace(comment.Body)) {
            throw new InvalidOperationException("Comment body is required");
        }
        if (items.Any(c  => c.Id == comment.Id)) {
            throw new InvalidOperationException("Comment with the same id already exists");
        }
        if (!_userRepository.GetMany().Any(u => u.Id == comment.UserId)) {
            throw new InvalidOperationException(
                "Post must be made by an existing user");
        }
        if (!_postRepository.GetMany().Any(p => p.PostId == comment.PostId)) {
            throw new InvalidOperationException("Post does not exist");
        }
    }
}