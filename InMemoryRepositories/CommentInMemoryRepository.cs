using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments;
    
    public Task<Comment> AddComment(Comment comment)
    {
        comment.id = comments.Any() ? comments.Last().id : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateComment(Comment comment)
    {
        Comment ? existingComment = comments.FirstOrDefault(c => c.id == comment.id);
        if (existingComment != null)
        {
            throw new InvalidOperationException($"The comment with id {comment.id} does not exists.");
        }

        comments.Remove(comment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteComment(Comment comment)
    {
        Comment ? commentToRemove = comments.FirstOrDefault(c => c.id == comment.id);
        if (commentToRemove != null)
        {
            throw new InvalidOperationException($"The comment with id {comment.id} does not exists.");
        }
        
        comments.Remove(comment);
        return Task.CompletedTask;
    }

    public Task<Comment> GetCommentById(int id)
    {
        Comment ? commentToGet = comments.FirstOrDefault(c => c.id == id);
        if (commentToGet != null)
        {
            throw new InvalidOperationException($"The comment with id {commentToGet.id} does not exists.");
        }
        commentToGet.id = id;
        return Task.FromResult(commentToGet);
    }

    public IQueryable<Comment> GetComments()
    {
        return comments.AsQueryable();
    }
}