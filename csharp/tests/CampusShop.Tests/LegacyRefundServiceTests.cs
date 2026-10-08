using NSubstitute;

namespace CampusShop.Tests;

/// <summary>退款，需求 4.11。</summary>
public class LegacyRefundServiceTests
{
    private readonly IPaymentGateway gateway = Substitute.For<IPaymentGateway>();

    [Fact]
    public void Refund_ValidAmount_CallsGatewayWithAmount()
    {
        gateway.Refund("u1", 30m).Returns(true);
        var svc = new LegacyRefundService(gateway);

        var result = svc.Refund("u1", 90m, 30m);

        Assert.True(result);
        gateway.Received(1).Refund("u1", 30m);
    }

    [Fact]
    public void Refund_EqualsPaid_CallsGateway()
    {
        gateway.Refund("u1", 90m).Returns(true);
        var svc = new LegacyRefundService(gateway);

        var result = svc.Refund("u1", 90m, 90m);

        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(90.01)]
    public void Refund_InvalidAmount_RejectedWithoutCallingGateway(double amount)
    {
        var svc = new LegacyRefundService(gateway);

        var result = svc.Refund("u1", 90m, (decimal)amount);

        Assert.False(result);
        gateway.DidNotReceive().Refund(Arg.Any<string>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Refund_GatewayFails_ReturnsFalse()
    {
        gateway.Refund("u1", 30m).Returns(false);
        var svc = new LegacyRefundService(gateway);

        var result = svc.Refund("u1", 90m, 30m);

        Assert.False(result);
    }
}
