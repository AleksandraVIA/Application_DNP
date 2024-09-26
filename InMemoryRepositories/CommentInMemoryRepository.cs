using Entities;
using RepositoryContracts;
using System.Collections.Generic;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : BaseInMemoryRepository<Comment>,
    ICommentRepository {
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CommentInMemoryRepository(IUserRepository userRepository, IPostRepository postRepository) {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
        
        items = new List<Comment> {
            new Comment (  1, "Comment 1" ,  1, 1),
            new Comment ( 2,  "Comment 2", 1, 1 ),
            new Comment (3, "Comment 3" , 1, 1),
            new Comment ( 4,  "Comment 4" ,  1,  1),
            new Comment ( 5, "Comment 5" , 1, 1)
        };
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
        if (!userRepository.GetMany().Any(u => u.Id == comment.UserId)) {
            throw new InvalidOperationException(
                "Post must be made by an existing user");
        }
        if (!postRepository.GetMany().Any(p => p.Id == comment.PostId)) {
            throw new InvalidOperationException("Post does not exist");
        }
    }
}