namespace CampusShop.Tests;

/// <summary>
/// 测试用的优惠券：满 100 减 20，日期相对于“今天”计算。
/// </summary>
public static class CouponFactory
{
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.Now);

    public static Coupon ExpiresInDays(int days, decimal threshold = 100m, bool used = false) => new()
    {
        Code = "TEST",
        Threshold = threshold,
        Amount = 20m,
        ExpireDate = Today.AddDays(days),
        IsUsed = used,
    };

    /// <summary>一个月后过期。</summary>
    public static Coupon Valid() => ExpiresInDays(30);

    /// <summary>今天是有效期最后一天。</summary>
    public static Coupon LastDayToday() => ExpiresInDays(0);

    /// <summary>昨天已过期。</summary>
    public static Coupon ExpiredYesterday() => ExpiresInDays(-1);
}
