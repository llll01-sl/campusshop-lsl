namespace CampusShop.Tests;

public class PriceCalculatorTests
{
    [Fact]
    public void ApplyDiscount_TotalIs0_Returns0()
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(0m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void ApplyDiscount_TotalIs50_Returns50()
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(50m);

        Assert.Equal(50m, result);
    }

    [Fact]
    public void ApplyDiscount_TotalIs99_Returns99()
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(99m);

        Assert.Equal(99m, result);
    }

    [Fact]
    public void ApplyDiscount_TotalIs120_Returns110()
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(120m);

        Assert.Equal(110m, result);
    }

    [Fact]
    public void ApplyDiscount_TotalIs150_Returns140()
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(150m);

        Assert.Equal(140m, result);
    }

    [Fact]
    public void ApplyDiscount_TotalIs180_Returns170()
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(180m);

        Assert.Equal(170m, result);
    }

    [Theory]
    [InlineData(200, 170)]       // 正好 200：减 30
    [InlineData(250, 220)]
    [InlineData(199.99, 189.99)] // 不足 200：仍按满 100 减 10
    public void ApplyDiscount_Tier200_Minus30(
        decimal total, decimal expected)
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(total);

        Assert.Equal(expected, result);
    }
}
