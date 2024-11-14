using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<GetPostResponseDto> CreatePostAsync(CreatePostDto request);
    public Task<GetPostResponseDto> GetPostAsync(int id);
    public Task<IResult> DeletePostAsync (DeletePost request, string? title, string? body, int id);
    public Task<GetCommentResponseDto> AddCommentAsync(CreateCommentDto request, int id);
}