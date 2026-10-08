namespace CampusShop.E2E;

public class LoginTests : ShopPageTest
{
    [Test]
    public async Task Login_ValidUser_ShowsWelcome()
    {
        // Arrange
        await Page.GotoAsync(BaseUrl);
        // Act
        await Page.GetByLabel("用户名").FillAsync("alice");
        await Page.GetByLabel("密码").FillAsync("123456");
        await Page.GetByRole(AriaRole.Button,
            new() { Name = "登录" }).ClickAsync();
        // Assert
        await Expect(Page.GetByText("欢迎，alice"))
            .ToBeVisibleAsync();
    }

    [Test]
    public async Task Login_WrongPassword_ShowsError()
    {
        await Page.GotoAsync(BaseUrl);

        await Page.GetByLabel("用户名").FillAsync("alice");
        await Page.GetByLabel("密码").FillAsync("wrong");
        await Page.GetByRole(AriaRole.Button,
            new() { Name = "登录" }).ClickAsync();

        await Expect(Page.GetByTestId("error-msg"))
            .ToHaveTextAsync("用户名或密码错误");
    }
}
