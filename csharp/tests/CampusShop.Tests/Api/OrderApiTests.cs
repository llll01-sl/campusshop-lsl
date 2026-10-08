using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CampusShop.Web;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CampusShop.Tests.Api;

/// <summary>
/// 下单与订单查询接口，需求 1.3 与第 4 节。每个测试自己登录、自己准备数据。
/// </summary>
public class OrderApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderApiTests(WebApplicationFactory<Program> factory)
        => _client = factory.CreateClient();

    // ---- 辅助方法 ----

    private async Task<string> LoginAs(string user)
    {
        var r = await _client.PostAsJsonAsync("/api/auth/login",
            new { username = user, password = "123456" });
        var body = await r.Content.ReadFromJsonAsync<LoginResponse>();
        return body!.Token;
    }

    private Task<HttpResponseMessage> PostOrder(string? token, string productId, int qty)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "/api/orders")
        {
            Content = JsonContent.Create(new { productId, quantity = qty }),
        };
        if (token != null)
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client.SendAsync(req);
    }

    private Task<HttpResponseMessage> GetOrder(string token, int id)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"/api/orders/{id}");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client.SendAsync(req);
    }

    private async Task<int> CreateOrderAs(string user)
    {
        var resp = await PostOrder(await LoginAs(user), "p2", 1);
        var order = await resp.Content.ReadFromJsonAsync<OrderDto>();
        return order!.Id;
    }

    // ---- 下单 ----

    [Fact]
    public async Task PostOrder_Valid_CreatesOrder()
    {
        var token = await LoginAs("alice");

        var resp = await PostOrder(token, "p1", 2);

        // 第 1 层：状态码
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        // 第 2 层：响应体（alice 是普通会员；单价 50 × 2 = 100 → 满 100 减 10）
        var order = await resp.Content.ReadFromJsonAsync<OrderDto>();
        Assert.Equal(90.00m, order!.Total);
        Assert.Equal("alice", order.User);
        // 第 3 层：副作用——能查到这个订单
        var get = await GetOrder(token, order.Id);
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task PostOrder_NoToken_Unauthorized()
    {
        var resp = await PostOrder(null, "p1", 1);

        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [Fact]
    public async Task PostOrder_OutOfStock_Conflict()
    {
        var token = await LoginAs("alice");

        var resp = await PostOrder(token, "p6", 1);   // p6 纪念徽章库存为 0

        Assert.Equal(HttpStatusCode.Conflict, resp.StatusCode);
    }

    [Fact]
    public async Task PostOrder_UnknownProduct_NotFound()
    {
        var token = await LoginAs("alice");

        var resp = await PostOrder(token, "nope", 1);

        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100)]
    public async Task PostOrder_InvalidQuantity_BadRequest(int qty)
    {
        var token = await LoginAs("alice");

        var resp = await PostOrder(token, "p1", qty);

        // 需求 4.6：数量必须是 1～99
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }

    // ---- 查询订单 ----

    [Fact]
    public async Task GetOrder_OwnOrder_Ok()
    {
        var token = await LoginAs("alice");

        var resp = await GetOrder(token, 1001);

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task GetOrder_OtherUsersOrder_Forbidden()
    {
        var bobOrderId = await CreateOrderAs("bob");
        var aliceToken = await LoginAs("alice");

        var resp = await GetOrder(aliceToken, bobOrderId);

        // 需求 4.7：不能查看他人的订单
        Assert.Contains(resp.StatusCode,
            new[] { HttpStatusCode.Forbidden, HttpStatusCode.NotFound });
    }

    [Fact]
    public async Task GetOrder_NotExist_NotFound()
    {
        var token = await LoginAs("alice");

        var resp = await GetOrder(token, 999999);

        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    // ---- 登录 ----

    [Fact]
    public async Task Login_WrongPassword_Unauthorized()
    {
        var resp = await _client.PostAsJsonAsync("/api/auth/login",
            new { username = "alice", password = "wrong" });

        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [Fact]
    public async Task Login_MissingUsername_BadRequest()
    {
        var resp = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new { password = "123456" });

        // 需求 1.3：用户名必填，缺少时返回 400
        Assert.Equal(HttpStatusCode.BadRequest,
                     resp.StatusCode);
    }
}
