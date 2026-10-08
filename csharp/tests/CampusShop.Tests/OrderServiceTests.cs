using NSubstitute;

namespace CampusShop.Tests;

/// <summary>下单流程，需求第 4 节。依赖都用 NSubstitute 替身，OrderService 是真实对象。</summary>
public class OrderServiceTests
{
    private readonly IInventoryRepository inv = Substitute.For<IInventoryRepository>();
    private readonly IPaymentGateway pay = Substitute.For<IPaymentGateway>();
    private readonly INotificationSender notify = Substitute.For<INotificationSender>();
    private readonly OrderService svc;

    public OrderServiceTests() => svc = new OrderService(pay, inv, notify);

    [Fact]
    public void PlaceOrder_AllSucceed_ChargesAndNotifies()
    {
        inv.GetStock("p1").Returns(10);
        pay.Charge("u1", Arg.Any<decimal>()).Returns(true);

        var result = svc.PlaceOrder(new Order("u1", "p1", 2, 90m));

        Assert.Equal(OrderStatus.Success, result.Status);
        Assert.False(string.IsNullOrEmpty(result.OrderId));
        inv.Received(1).Deduct("p1", 2);
        pay.Received(1).Charge("u1", 90m);     // 金额正确
        notify.Received(1).Send(Arg.Is<Notification>(n => n.UserId == "u1"));
    }

    [Fact]
    public void PlaceOrder_InsufficientStock_DoesNotCharge()
    {
        inv.GetStock("p1").Returns(1);

        var result = svc.PlaceOrder(new Order("u1", "p1", 2, 90m));

        // 需求 4.1：库存不足，不得扣款
        Assert.Equal(OrderStatus.InsufficientStock, result.Status);
        inv.DidNotReceive().Deduct(Arg.Any<string>(), Arg.Any<int>());
        pay.DidNotReceive().Charge(Arg.Any<string>(), Arg.Any<decimal>());
        notify.DidNotReceive().Send(Arg.Any<Notification>());
    }

    [Fact]
    public void PlaceOrder_StockEqualsQuantity_Succeeds()
    {
        inv.GetStock("p1").Returns(2);
        pay.Charge("u1", Arg.Any<decimal>()).Returns(true);

        var result = svc.PlaceOrder(new Order("u1", "p1", 2, 90m));

        Assert.Equal(OrderStatus.Success, result.Status);
    }

    [Fact]
    public void PlaceOrder_PaymentFails_RestoresStock()
    {
        inv.GetStock("p1").Returns(10);
        pay.Charge("u1", Arg.Any<decimal>()).Returns(false);

        var result = svc.PlaceOrder(new Order("u1", "p1", 2, 90m));

        // 需求 4.3：支付失败 → 恢复库存
        Assert.Equal(OrderStatus.PaymentFailed, result.Status);
        inv.Received(1).Restore("p1", 2);
        notify.DidNotReceive().Send(Arg.Any<Notification>());
    }

    [Fact]
    public void PlaceOrder_NotifyFails_StillSucceeds()
    {
        inv.GetStock("p1").Returns(10);
        pay.Charge("u1", Arg.Any<decimal>()).Returns(true);
        notify.When(n => n.Send(Arg.Any<Notification>()))
              .Do(_ => throw new TimeoutException());

        var result = svc.PlaceOrder(new Order("u1", "p1", 2, 90m));

        // 需求 4.4：通知失败不影响订单成功
        Assert.Equal(OrderStatus.Success, result.Status);
        inv.DidNotReceive().Restore(Arg.Any<string>(), Arg.Any<int>());
    }
}
