package campusshop;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyInt;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.argThat;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.doThrow;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/** 下单流程，需求第 4 节。依赖都用 Mockito 替身，OrderService 是真实对象。 */
@ExtendWith(MockitoExtension.class)
class OrderServiceTest {

    @Mock InventoryRepository inv;
    @Mock PaymentGateway pay;
    @Mock NotificationSender notify;
    @InjectMocks OrderService svc;

    private final Order order = new Order("u1", "p1", 2, new BigDecimal("90"));

    @Test
    void placeOrder_allSucceed_chargesAndNotifies() {
        when(inv.getStock("p1")).thenReturn(10);
        when(pay.charge(eq("u1"), any())).thenReturn(true);

        OrderResult r = svc.placeOrder(order);

        assertEquals(OrderStatus.SUCCESS, r.getStatus());
        assertNotNull(r.getOrderId());
        verify(inv).deduct("p1", 2);
        verify(pay).charge("u1", new BigDecimal("90"));     // 金额正确
        verify(notify).send(argThat(n -> n.userId().equals("u1")));
    }

    @Test
    void placeOrder_insufficientStock_doesNotCharge() {
        when(inv.getStock("p1")).thenReturn(1);

        OrderResult r = svc.placeOrder(order);

        // 需求 4.1：库存不足，不得扣款
        assertEquals(OrderStatus.INSUFFICIENT_STOCK, r.getStatus());
        verify(inv, never()).deduct(anyString(), anyInt());
        verify(pay, never()).charge(anyString(), any());
        verify(notify, never()).send(any());
    }

    @Test
    void placeOrder_stockEqualsQuantity_succeeds() {
        when(inv.getStock("p1")).thenReturn(2);
        when(pay.charge(eq("u1"), any())).thenReturn(true);

        OrderResult r = svc.placeOrder(order);

        assertEquals(OrderStatus.SUCCESS, r.getStatus());
    }

    @Test
    void placeOrder_paymentFails_restoresStock() {
        when(inv.getStock("p1")).thenReturn(10);
        when(pay.charge(eq("u1"), any())).thenReturn(false);

        OrderResult r = svc.placeOrder(order);

        // 需求 4.3：支付失败 → 恢复库存
        assertEquals(OrderStatus.PAYMENT_FAILED, r.getStatus());
        verify(inv).restore("p1", 2);
        verify(notify, never()).send(any());
    }

    @Test
    void placeOrder_notifyFails_stillSucceeds() {
        when(inv.getStock("p1")).thenReturn(10);
        when(pay.charge(eq("u1"), any())).thenReturn(true);
        doThrow(new RuntimeException("timeout"))
                .when(notify).send(any());

        OrderResult r = svc.placeOrder(order);

        // 需求 4.4：通知失败不影响订单成功
        assertEquals(OrderStatus.SUCCESS, r.getStatus());
        verify(inv, never()).restore(anyString(), anyInt());
    }
}
