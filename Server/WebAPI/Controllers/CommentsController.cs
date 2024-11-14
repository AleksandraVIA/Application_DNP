using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
    }


    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromBody] CreateCommentDto request) {
        Comment comment = new Comment {
            Body = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };
        Comment createdComment = await commentRepo.AddAsync(comment);
        CommentDto commentDto = new() {
            Id = createdComment.Id,
            Body = createdComment.Body,
            PostId = createdComment.PostId,
            AuthorsName = userRepo
                .GetSingleAsync(createdComment.UserId).Result.Username
        };
        return Created($"/Comments/{commentDto.Id}", commentDto);
    }

    
    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        Comment existingComment = await commentRepo.GetSingleAsync(id);
        if (existingComment == null) return NotFound();

        existingComment.Body = request.Body;
        existingComment.PostId = request.PostId;
        existingComment.UserId = request.UserId;

        await commentRepo.UpdateAsync(existingComment);

        CommentDto dto = new CommentDto
        {
            Id = existingComment.Id,
            Body = existingComment.Body,
            PostId = existingComment.PostId,
            UserId = existingComment.UserId
        };
        return Ok(dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? postId) {
        IEnumerable<Comment> comments = commentRepo.GetMany();

        if (postId.HasValue) {
            comments =
                comments.Where(comment => comment.PostId == postId.Value);
        }

        IEnumerable<CommentDto> commentDtos = comments.Select(comment =>
            new CommentDto {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                AuthorsName = userRepo.GetSingleAsync(comment.UserId)
                    .Result.Username
            });
        return Ok(commentDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id) {
        Comment comment = await commentRepo.GetSingleAsync(id);
        CommentDto commentDto = new() {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            AuthorsName = userRepo.GetSingleAsync(comment.UserId)
                .Result.Username
        };
        return Ok(commentDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromQuery] string search = "")
    {
        IEnumerable<Comment> comments = commentRepo.GetMany().ToList();

        List<CommentDto> commentDtos = new List<CommentDto>();
        foreach (var comment in comments)
        {
            commentDtos.Add(new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            });
        }

        return Ok(commentDtos);
    }
    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id) {
        await commentRepo.DeleteAsync(id);
        return NoContent();
    }
}
