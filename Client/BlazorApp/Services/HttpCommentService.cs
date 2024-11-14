using System.Text;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }
    

    public async Task<CreateCommentDto> AddCommentAsync(CommentDto request)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent content = new(requestJson, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PostAsync("Comments", content);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            throw new Exception(responseContent);
        }
        CreateCommentDto createdComment = JsonSerializer.Deserialize<CreateCommentDto>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;

        return createdComment;
    }


    public async Task<List<GetCommentResponseDto>> GetAllCommentssync(CommentDto request)
    {
        HttpResponseMessage response = await client.GetAsync("Comments");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        List<GetCommentResponseDto> receivedDto =
            JsonSerializer.Deserialize<List<GetCommentResponseDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

        return receivedDto;
    }

    public async Task<IResult> DeleteCommentAsync(DeleteCommentDto request, int id)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent=new StringContent(requestJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.SendAsync(
            new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Content = stringContent,
               RequestUri = new Uri($"http://localhost:5118/Comments/{id}")
            });
        String content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }
        return Results.NoContent();
    }
}