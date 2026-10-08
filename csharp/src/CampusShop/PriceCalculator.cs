namespace CampusShop;

/// <summary>
/// 订单计价。规则见 docs/requirements.md 第 2 节。
/// </summary>
public class PriceCalculator
{
    /// <summary>
    /// 满减：满 200 元减 30 元；满 100 元减 10 元。取最高一档，不叠加。
    /// </summary>
    public decimal ApplyDiscount(decimal total)
    {
        if (total >= 200)
            return total - 30;
        if (total >= 100)
            return total - 10;
        return total;
    }

    /// <summary>
    /// 计算应付金额：先按会员等级打折，再按折后金额满减。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">total 小于 0 或大于 100000。</exception>
    public decimal CalculateTotal(decimal total, MemberLevel level)
    {
        if (total < 0 || total > 100000)
            throw new ArgumentOutOfRangeException(nameof(total));

        decimal discounted = Math.Round(total * Rate(level), 2, MidpointRounding.AwayFromZero);
        if (discounted >= 200)
            return discounted - 30;
        if (discounted >= 100)
            return discounted - 10;
        return discounted;
    }

    private static decimal Rate(MemberLevel level) => level switch
    {
        MemberLevel.Silver => 0.95m,
        MemberLevel.Gold => 0.9m,
        _ => 1m,
    };
}
