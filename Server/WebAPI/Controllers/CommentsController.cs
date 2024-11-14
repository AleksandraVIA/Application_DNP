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
    public ActionResult<IEnumerable<CommentDto>> GetComments([FromQuery] int? postId)
    {
        IEnumerable<Comment> comments = commentRepo.GetMany();

        if (postId.HasValue)
        {
            comments = comments.Where(comment => comment.PostId == postId.Value);
        }

        IEnumerable<CommentDto> commentDtos = comments.Select(comment =>
            new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                AuthorsName = userRepo.GetSingleAsync(comment.UserId).Result.Username
            });
        return Ok(commentDtos);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> SearchComments([FromQuery] string search = "")
    {
        IEnumerable<Comment> comments = commentRepo.GetMany();

        List<CommentDto> commentDtos = comments
            .Where(c => c.Body.Contains(search)) // Add search criteria
            .Select(comment => new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            }).ToList();

        return Ok(commentDtos);
    }
}
