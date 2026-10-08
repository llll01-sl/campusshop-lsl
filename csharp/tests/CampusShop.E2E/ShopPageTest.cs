namespace CampusShop.E2E;

/// <summary>E2E 测试的基类：提供网站地址、登录步骤，并为每个测试记录 Trace。</summary>
public abstract class ShopPageTest : PageTest
{
    protected const string BaseUrl = "http://localhost:5000";

    [SetUp]
    public async Task StartTrace() =>
        await Context.Tracing.StartAsync(new() {
            Screenshots = true, Snapshots = true, Sources = true });

    [TearDown]
    public async Task StopTrace() =>
        await Context.Tracing.StopAsync(new() {
            Path = Path.Combine("traces",
                $"{TestContext.CurrentContext.Test.Name}.zip") });

    /// <summary>用演示账号登录（在密码框按回车提交），并等待欢迎信息出现。</summary>
    protected async Task LoginAs(string user)
    {
        await Page.GotoAsync(BaseUrl);
        await Page.GetByLabel("用户名").FillAsync(user);
        await Page.GetByLabel("密码").FillAsync("123456");
        await Page.GetByLabel("密码").PressAsync("Enter");
        await Expect(Page.GetByText($"欢迎，{user}")).ToBeVisibleAsync();
    }
}
