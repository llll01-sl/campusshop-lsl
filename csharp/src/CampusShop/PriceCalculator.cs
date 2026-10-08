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
        if (total > 100)
            return total - 10;
        return total;
    }
}
