namespace CampusShop.Tests;

public class CouponServiceTests
{
    [Fact]
    public void IsUsable_ValidCoupon_ReturnsTrue()
    {
        // Arrange
        var service = new CouponService();
        var coupon = CouponFactory.Valid();

        // Act
        var result = service.IsUsable(coupon, 150m);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUsable_ExpiredCoupon_ReturnsFalse()
    {
        // Arrange
        var service = new CouponService();
        var coupon = CouponFactory.ExpiredYesterday();

        // Act
        var result = service.IsUsable(coupon, 150m);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsUsable_BelowThreshold_ReturnsFalse()
    {
        var service = new CouponService();
        var coupon = CouponFactory.Valid();

        var result = service.IsUsable(coupon, 99.99m);

        Assert.False(result);
    }

    [Fact]
    public void IsUsable_ExactlyThreshold_ReturnsTrue()
    {
        var service = new CouponService();
        var coupon = CouponFactory.Valid();

        var result = service.IsUsable(coupon, 100m);

        Assert.True(result);
    }
}
