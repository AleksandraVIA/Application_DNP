using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiContracts;
using Microsoft.AspNetCore.Identity.Data;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private ClaimsPrincipal currentClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity()); // Unauthenticated by default

    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Login(string userName, string password)
    {
        var loginRequest = new CustomLoginRequest
        {
            Username = userName,
            Password = password
        };

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("auth/login", loginRequest);
        string content = await response.Content.ReadAsStringAsync();
    
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim("Id", userDto.Id.ToString()),
            new Claim("Password", userDto.Password)
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        currentClaimsPrincipal = new ClaimsPrincipal(identity);
    
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(currentClaimsPrincipal));
    }

    public async Task Logout()
    {
        // Clear the current user claims
        currentClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
}
