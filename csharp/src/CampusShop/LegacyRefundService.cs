namespace CampusShop;

/// <summary>
/// 退款（旧代码）。见 docs/requirements.md 4.11。
/// </summary>
public class LegacyRefundService
{
    /// <summary>
    /// 为用户退款。paid 为订单实付金额，amount 为本次退款金额。
    /// 退款成功返回 true。
    /// </summary>
    public bool Refund(string userId, decimal paid, decimal amount)
    {
        if (amount <= 0 || amount > paid)
            return false;

        var gateway = new AlipayGateway();
        return gateway.Refund(userId, amount);
    }
}
