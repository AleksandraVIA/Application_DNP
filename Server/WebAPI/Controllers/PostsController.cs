
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }

    
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        Post post = new Post();
        Post created = await postRepo.AddAsync(post);
        
        PostDto dto = new PostDto
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId,
            Comments = new List<CommentDto>() 
        };

        return Created($"/Posts/{dto.Id}", dto);
    }

    // Update Post (PUT)
    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        Post existingPost = await postRepo.GetSingleAsync(id);
        if (existingPost == null) return NotFound();

        existingPost.Title = request.Title;
        existingPost.Body = request.Body;
        existingPost.UserId = request.UserId;
        
        await postRepo.UpdateAsync(existingPost);

        PostDto dto = new PostDto
        {
            Id = existingPost.Id,
            Title = existingPost.Title,
            Body = existingPost.Body,
            UserId = existingPost.UserId,
            Comments = new List<CommentDto>() 
        };
        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id) {
        Post post = await postRepo.GetSingleAsync(id);
        PostDto postDto = new() {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            Author = userRepo.GetSingleAsync(post.UserId).Result.Username
        };
        return Ok(postDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts([FromQuery] string? title) {
        IEnumerable<Post> posts = postRepo.GetMany();

        if (title != null) {
            posts = posts.Where(post => post.Title.Contains(title));
        }
        IEnumerable<PostDto> postDtos = posts.Select(post => new PostDto {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            Author = userRepo.GetSingleAsync(post.UserId).Result.Username
        });
        return Ok(postDtos);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id) {
        await postRepo.DeleteAsync(id);
        return NoContent();
    }
}
