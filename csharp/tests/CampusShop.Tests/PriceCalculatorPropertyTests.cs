using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace CampusShop.Tests;

/// <summary>
/// 属性测试：FsCheck 随机生成 100 组输入，检查需求保证的规律是否总是成立。
/// </summary>
public class PriceCalculatorPropertyTests
{
    // 随机生成 0 ~ 10000000 “分”，即 0 ~ 100000 元（需求 2.1 的取值范围）
    private static readonly Arbitrary<int> Cents = Gen.Choose(0, 10_000_000).ToArbitrary();

    [Property]
    public Property Total_NeverNegative_NeverExceedsOriginal() =>
        Prop.ForAll(Cents, cents =>
        {
            var total = cents / 100m;
            var calc = new PriceCalculator();

            return Enum.GetValues<MemberLevel>().All(level =>
            {
                var result = calc.CalculateTotal(total, level);
                return result >= 0 && result <= total;
            });
        });

    [Property]
    public Property ApplyDiscount_ReducesBy0Or10Or30() =>
        Prop.ForAll(Cents, cents =>
        {
            var total = cents / 100m;

            var reduction = total - new PriceCalculator().ApplyDiscount(total);

            // 需求 2.3：满减只有 0、10、30 三档
            return reduction == 0 || reduction == 10 || reduction == 30;
        });

    // 课堂发现：“金卡永远不比银卡贵”并不成立（例如原价 110 元：金卡 99.00，银卡 94.50）。
    // 这是需求的漏洞，不是代码缺陷，已记录在 requirements.md 2.6，等待产品确认，因此这里不写成测试。
}
