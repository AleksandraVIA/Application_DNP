using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CreateCommentDto> AddCommentAsync(CommentDto request);
    public Task<List<GetCommentResponseDto>> GetAllCommentssync(CommentDto request);
    public Task<IResult> DeleteCommentAsync(DeleteCommentDto request, int id);
}