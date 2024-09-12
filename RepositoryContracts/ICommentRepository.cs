using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddComment(Comment comment);
    Task UpdateComment(Comment comment);
    Task DeleteComment(Comment comment);
    Task<Comment> GetCommentById(int id);
    IQueryable<Comment> GetComments();
    
}