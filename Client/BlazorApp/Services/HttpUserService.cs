using System.Text;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public class HttpUserService:IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }


    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent content = new(requestJson, Encoding.UTF8, "application/json");

        Console.WriteLine("Username http: " + request.UserName + "; Password: " + request.Password);

        HttpResponseMessage response = await client.PostAsync("Users", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.Content},         {response.ReasonPhrase},      {content}");
            throw new Exception(responseContent);
        }

        AddUserResponseDto receivedDto = JsonSerializer.Deserialize<AddUserResponseDto>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        UserDto userDto = new UserDto
        {
            Id = receivedDto.UserId,
            Username = receivedDto.Username,
            Password = receivedDto.Password,
            Email = receivedDto.Email
        };

        return userDto;
    }
    

    public async Task<AddUserResponseDto> GetUserAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"/Users/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        AddUserResponseDto receivedDto = JsonSerializer.Deserialize<AddUserResponseDto>(
            content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return receivedDto;
    }

    public async Task<List<string>> GetAllUsersAsync(string? nameContains)
    {
        string requestUri = "Users";
        
        if (!string.IsNullOrWhiteSpace(nameContains))
            requestUri += $"?nameContains={Uri.EscapeDataString(nameContains.Trim())}";
        
        HttpResponseMessage response = await client.GetAsync(requestUri);
        string content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }
        
        var usernames = JsonSerializer.Deserialize<List<string>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return usernames ?? new List<string>();
    }
    

    public async Task<IResult> DeleteUserAsync(DeleteUserDto request, int id)
    {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent stringContent = new StringContent(requestJson,
            Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(client.BaseAddress, $"Users/{id}"),
            Content = stringContent
        });
        String content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, {content}");
            throw new Exception($"Error: {response.ReasonPhrase}");
        }
        return Results.NoContent();
    }
    
}