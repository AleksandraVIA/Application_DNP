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

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }


    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        
        Comment comment = new Comment(request.Body, request.PostId, request.UserId); 
        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = new CommentDto
        {
            Id = created.Id, 
            Body = created.Body,
            PostId = created.PostId,
            UserId = created.UserId
        };

        return Created($"/Comments/{dto.Id}", dto);
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

    // Get Single Comment (GET by ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        Comment comment = await commentRepo.GetSingleAsync(id);
        if (comment == null) return NotFound();

        CommentDto dto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        };
        return Ok(dto);
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
    public async Task<IActionResult> DeleteComment(int id)
    {
        Comment comment = await commentRepo.GetSingleAsync(id);
        if (comment == null) return NotFound();

        await commentRepo.DeleteAsync(id);
        return NoContent(); 
    }
}
