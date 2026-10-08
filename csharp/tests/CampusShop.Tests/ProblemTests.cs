namespace CampusShop.Tests;

// 第 6 周课堂练习：这个文件中的测试都有问题，请找出来。
public class ProblemTests
{
    private readonly PriceCalculator calc = new();

    [Fact] public void Test1()
    { calc.CalculateTotal(150m, MemberLevel.Normal); }

    [Fact] public void Test2()
    { Assert.NotNull(calc.CalculateTotal(150m, MemberLevel.Gold)); }

    [Fact] public void Test3()
    { var t = 250m; var exp = t >= 200 ? t - 30 : t;
      Assert.Equal(exp, calc.CalculateTotal(t, MemberLevel.Normal)); }

    [Fact] public void Test4()
    { try { calc.CalculateTotal(-1m, MemberLevel.Normal); }
      catch (Exception) { } }

    [Fact(Skip = "flaky")] public void Test5()
    { Assert.Equal(94.50m, calc.CalculateTotal(110m, MemberLevel.Silver)); }
}
