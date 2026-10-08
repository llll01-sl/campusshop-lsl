namespace CampusShop;

/// <summary>
/// 支付宝网关。真实环境中每次调用都要访问支付宝服务器；
/// 教学项目中没有真实的支付接口，调用时直接抛出异常。
/// </summary>
public class AlipayGateway : IPaymentGateway
{
    public bool Charge(string userId, decimal amount) =>
        throw new InvalidOperationException("无法连接支付宝服务器");

    public bool Refund(string userId, decimal amount) =>
        throw new InvalidOperationException("无法连接支付宝服务器");
}
