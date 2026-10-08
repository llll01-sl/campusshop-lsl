namespace CampusShop;

/// <summary>
/// 退款。见 docs/requirements.md 4.11。
/// 支付网关通过构造函数注入：生产环境传入 AlipayGateway，测试中传入测试替身。
/// </summary>
public class LegacyRefundService
{
    private readonly IPaymentGateway _gateway;

    public LegacyRefundService(IPaymentGateway gateway) => _gateway = gateway;

    /// <summary>
    /// 为用户退款。paid 为订单实付金额，amount 为本次退款金额。
    /// 退款成功返回 true。
    /// </summary>
    public bool Refund(string userId, decimal paid, decimal amount)
    {
        if (amount <= 0 || amount > paid)
            return false;

        return _gateway.Refund(userId, amount);
    }
}
