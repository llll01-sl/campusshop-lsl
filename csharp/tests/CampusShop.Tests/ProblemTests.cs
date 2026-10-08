namespace CampusShop.Tests;

// 第 6 周课堂练习的修正版：每个测试都有明确的断言，期望值按需求手算。
public class ProblemTests
{
    private readonly PriceCalculator calc = new();

    [Fact]
    public void Test1_Total150_Normal_Returns140()
    {
        var result = calc.CalculateTotal(150m, MemberLevel.Normal);

        // 需求 2.3：150 ≥ 100，减 10 → 140.00
        Assert.Equal(140.00m, result);
    }

    [Fact]
    public void Test2_Total150_Gold_Returns125()
    {
        var result = calc.CalculateTotal(150m, MemberLevel.Gold);

        // 需求 2.2、2.3：150 × 0.9 = 135，满 100 减 10 → 125.00
        Assert.Equal(125.00m, result);
    }

    [Fact]
    public void Test3_Total250_Normal_Returns220()
    {
        var result = calc.CalculateTotal(250m, MemberLevel.Normal);

        // 需求 2.3：250 ≥ 200，减 30 → 220.00
        Assert.Equal(220.00m, result);
    }

    [Fact]
    public void Test4_NegativeTotal_Throws()
    {
        // 需求 2.1：原价小于 0 时抛出参数越界异常
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => calc.CalculateTotal(-1m, MemberLevel.Normal));

        Assert.Equal("total", ex.ParamName);
    }

    [Fact]
    public void Test5_Total110_Silver_Returns94_50()
    {
        var result = calc.CalculateTotal(110m, MemberLevel.Silver);

        // 需求 2.2、2.3：110 × 0.95 = 104.50，满 100 减 10 → 94.50
        Assert.Equal(94.50m, result);
    }
}
