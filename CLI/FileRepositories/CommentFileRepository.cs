using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            AddAsync();
        }
        
    }
    
    public async Task AddAsync()
    {
        int delay = 0;
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = "I won't miss it. Tak for det.", PostId = 1,
            UserId = 2
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "I have DNP at that time, I won't see it, oh nej.",
            PostId = 1, UserId = 3
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = "Remember, the Earth is flat.", PostId = 1,
            UserId = 2
        });


        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = "Do you guys have a dad?", PostId = 2, UserId = 5
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
            { Body = "I have 2 dads.", PostId = 2, UserId = 2 });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "What did one plate whisper to the other plate? A: Dinner is on me.",
            PostId = 2, UserId = 1
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "What do you call cheese that’s not your cheese? A: Nacho cheese.",
            PostId = 2, UserId = 3
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = "I made you read my comment haha.", PostId = 3,
            UserId = 4
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "Kevin Dutton - The good psychopath's guide to success.",
            PostId = 4, UserId = 4
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = "Giorgio Faletti - I kill.", PostId = 4,
            UserId = 2
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "I dont read, I'm to smart for that. Do smth useful with you're time. The writer's just wanna sell they're books, but I dont put my money their.",
            PostId = 4, UserId = 2
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "DNP is interesting and I'm not saying this because other people are reading my comment. ",
            PostId = 5, UserId = 3
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
            { Body = "Aah nej! ", PostId = 5, UserId = 2 });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
            { Body = "Who cares?", PostId = 6, UserId = 4 });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "Put aluminium foil in the microwave, turn it on for 2 minutes, take it out and smell it. Works 100%",
            PostId = 7, UserId = 1
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "Yes, the solution @betelgeuse suggested is great.",
            PostId = 7, UserId = 2
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "I was skeptical about it, but it just took my pain away. Amazing!",
            PostId = 7, UserId = 4
        });
        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "I can't believe it works! It works faster if you use more aluminium foil.",
            PostId = 7, UserId = 3
        });

        Thread.Sleep(delay);
        await AddCommentAsync(new Comment
        {
            Body = 
                "Water might not be wet. This is because most scientists define wetness as a liquid’s ability to maintain contact with a solid surface, meaning that water itself is not wet, but can make other objects wet.",
            PostId = 8, UserId = 4
        });
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxId + 1;
        comments.Add(comment);
        SaveCommentsAsync(comments);
        return comment;
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Comment comment)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment commentToUpdate = await GetSingleAsync(comment.Id);
        comments.Remove(commentToUpdate);
        comments.Add(comment);
        SaveCommentsAsync(comments);
    }

    public async Task DeleteAsync(int commentId)
    {

        List<Comment> comments = await LoadCommentsAsync();
        Comment commentToDelete = await GetSingleAsync(commentId);
        foreach(Comment comment in comments.ToList())
        {
            if (comment.Id == commentToDelete.Id)
                comments.Remove(comment);
        }
        SaveCommentsAsync(comments);

    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        List<Comment> comments = await LoadCommentsAsync();
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
            throw new InvalidOperationException("No comment found");
        return comment;
    }
    

    public IQueryable<Comment> GetMany()
    {
        List<Comment> comments = LoadCommentsAsync().Result;

        return comments.AsQueryable();
    }

    private async Task<List<Comment>> LoadCommentsAsync()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments;
    }

    private async void SaveCommentsAsync(List<Comment> toSaveComments)
    {
        string commentsAsJson = JsonSerializer.Serialize(toSaveComments,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }
}