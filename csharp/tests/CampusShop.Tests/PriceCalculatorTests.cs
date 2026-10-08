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

    // ---- CalculateTotal：需求第 2 节 ----

    [Theory]
    [InlineData(99.99,  MemberLevel.Normal, 99.99)]  // 边界 100 左
    [InlineData(100,    MemberLevel.Normal, 90.00)]  // 边界 100
    [InlineData(200,    MemberLevel.Normal, 170.00)] // 边界 200
    [InlineData(200,    MemberLevel.Gold,   170.00)] // 交互：200 × 0.9 = 180 → 减 10
    [InlineData(110,    MemberLevel.Silver, 94.50)]  // 交互：110 × 0.95 = 104.5 → 减 10
    [InlineData(105,    MemberLevel.Silver, 99.75)]  // 交互：105 × 0.95 = 99.75，跌破门槛
    public void CalculateTotal_ValidInput_ReturnsExpected(
        double total, MemberLevel level, double expected)
    {
        var calc = new PriceCalculator();

        var result = calc.CalculateTotal((decimal)total, level);

        Assert.Equal((decimal)expected, result);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(100000.01)]
    public void CalculateTotal_OutOfRange_Throws(double total)
    {
        var calc = new PriceCalculator();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => calc.CalculateTotal((decimal)total, MemberLevel.Normal));

        Assert.Equal("total", ex.ParamName);
    }

    [Fact]
    public void ApplyDiscount_TotalIs100_Returns90()
    {
        var calc = new PriceCalculator();

        var result = calc.ApplyDiscount(100m);

        Assert.Equal(90m, result);
    }

    // ---- 第 8 周：针对存活的变异体补充的测试 ----

    [Theory]
    [InlineData(0,      MemberLevel.Normal, 0.00)]     // 需求 2.1：0 是合法下限（杀死 < → <=）
    [InlineData(100000, MemberLevel.Normal, 99970.00)] // 需求 2.1：100000 是合法上限（杀死 > → >=）
    [InlineData(133.33, MemberLevel.Silver, 116.66)]   // 需求 2.2：133.33×0.95=126.6635→126.66→减10→116.66
    [InlineData(150,    MemberLevel.Gold,   125.00)]   // 需求 2.2：150×0.9=135→减10→125
    [InlineData(10.10,  MemberLevel.Silver, 9.60)]     // 需求 2.2：10.10×0.95=9.595→四舍五入→9.60
    public void CalculateTotal_BoundaryAndRounding_ReturnsExpected(
        double total, MemberLevel level, double expected)
    {
        var calc = new PriceCalculator();

        var result = calc.CalculateTotal((decimal)total, level);

        Assert.Equal((decimal)expected, result);
    }
}
