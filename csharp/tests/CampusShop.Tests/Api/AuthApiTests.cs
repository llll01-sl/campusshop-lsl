using System.Net;
using System.Net.Http.Json;
using CampusShop.Web;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CampusShop.Tests.Api;

/// <summary>
/// API 测试示例：WebApplicationFactory 在内存中启动整个网站，不需要先 dotnet run。
/// </summary>
public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthApiTests(WebApplicationFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task Login_ValidUser_ReturnsToken()
    {
        var resp = await _client.PostAsJsonAsync("/api/auth/login",
            new { username = "alice", password = "123456" });

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var body = await resp.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.False(string.IsNullOrEmpty(body!.Token));
        Assert.Equal(3600, body.ExpiresIn);
    }
}
