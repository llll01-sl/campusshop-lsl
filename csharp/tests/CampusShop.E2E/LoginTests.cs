namespace CampusShop.E2E;

public class LoginTests : PageTest
{
    [Test]
    public async Task Login_ValidUser_ShowsWelcome()
    {
        // Arrange
        await Page.GotoAsync("http://localhost:5000");
        // Act
        await Page.GetByLabel("用户名").FillAsync("alice");
        await Page.GetByLabel("密码").FillAsync("123456");
        await Page.GetByRole(AriaRole.Button,
            new() { Name = "登录" }).ClickAsync();
        // Assert
        await Expect(Page.GetByText("欢迎，alice"))
            .ToBeVisibleAsync();
    }
}
