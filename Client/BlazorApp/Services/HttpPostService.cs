using System.Text;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<GetPostResponseDto> CreatePostAsync(CreatePostDto request)
    {
        String requestJson = JsonSerializer.Serialize(request);
        StringContent content =
            new(requestJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("Posts", content);
        String responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetPostResponseDto receivedDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }

    public async Task<GetPostResponseDto> GetPostAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"Posts/{id}");
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetPostResponseDto receivedDto =
            JsonSerializer.Deserialize<GetPostResponseDto>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }
    

    public async Task<List<GetPostResponseDto>> GetPostsAsync(
        string? author)
    {
        string requestUri = "Posts";
        if (!string.IsNullOrWhiteSpace(author))
            requestUri += $"?author={Uri.EscapeDataString(author.Trim())}";

        HttpResponseMessage response = await client.GetAsync(requestUri);
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        List<GetPostResponseDto> receivedDto =
            JsonSerializer.Deserialize<List<GetPostResponseDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }
    

    public async Task<IResult> DeletePostAsync(DeletePost request, string? title, string? body, int id)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent = new StringContent(requestJson,
            Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.SendAsync(
            new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(client.BaseAddress,
                    $"/Posts/{id}"),
                Content = stringContent
            });
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        return Results.NoContent();
    }
    

    public async Task<GetCommentResponseDto> AddCommentAsync(CreateCommentDto request, int id)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response =
            await client.PostAsync($"Posts/{id}/Comments", stringContent);
        string content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        GetCommentResponseDto receivedDto = JsonSerializer.Deserialize<GetCommentResponseDto>(content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return receivedDto;
    }
}