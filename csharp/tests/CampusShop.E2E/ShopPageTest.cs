namespace CampusShop.E2E;

/// <summary>E2E 测试的基类：提供网站地址和登录步骤。</summary>
public abstract class ShopPageTest : PageTest
{
    protected const string BaseUrl = "http://localhost:5000";

    /// <summary>用演示账号登录，并等待欢迎信息出现。</summary>
    protected async Task LoginAs(string user)
    {
        await Page.GotoAsync(BaseUrl);
        await Page.GetByLabel("用户名").FillAsync(user);
        await Page.GetByLabel("密码").FillAsync("123456");
        await Page.GetByRole(AriaRole.Button, new() { Name = "登录" }).ClickAsync();
        await Expect(Page.GetByText($"欢迎，{user}")).ToBeVisibleAsync();
    }
}
