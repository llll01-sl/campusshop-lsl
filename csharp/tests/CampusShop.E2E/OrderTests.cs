namespace CampusShop.E2E;

public class OrderTests : ShopPageTest
{
    [Test]
    public async Task Order_TwoNotebooks_ShowsOrderNumber()
    {
        await LoginAs("alice");
        var row = Page.GetByRole(AriaRole.Row,
            new() { Name = "笔记本" });
        await row.GetByLabel("数量").FillAsync("2");
        await row.GetByRole(AriaRole.Button,
            new() { Name = "加入购物车" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link,
            new() { Name = "购物车" }).ClickAsync();
        // alice 是普通会员：50 × 2 = 100，满 100 减 10
        await Expect(Page.GetByTestId("cart-total"))
            .ToHaveTextAsync("¥90.00");
        await Page.GetByRole(AriaRole.Button,
            new() { Name = "提交订单" }).ClickAsync();
        await Expect(Page.GetByTestId("order-no"))
            .ToHaveTextAsync(new Regex(@"^CS\d+$"));
    }

    [Test]
    public async Task Cart_Empty_OrderButtonDisabled()
    {
        await LoginAs("bob");   // bob 的购物车为空
        await Page.GetByRole(AriaRole.Link,
            new() { Name = "购物车" }).ClickAsync();

        await Expect(Page.GetByText("购物车为空"))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button,
            new() { Name = "提交订单" })).ToBeDisabledAsync();
    }
}
