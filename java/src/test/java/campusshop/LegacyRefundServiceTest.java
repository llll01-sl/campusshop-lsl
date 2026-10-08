package campusshop;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/** 退款，需求 4.11。 */
@ExtendWith(MockitoExtension.class)
class LegacyRefundServiceTest {

    private static final BigDecimal PAID = new BigDecimal("90");

    @Mock PaymentGateway gateway;

    @Test
    void refund_validAmount_callsGatewayWithAmount() {
        when(gateway.refund("u1", new BigDecimal("30"))).thenReturn(true);
        LegacyRefundService svc = new LegacyRefundService(gateway);

        boolean result = svc.refund("u1", PAID, new BigDecimal("30"));

        assertTrue(result);
        verify(gateway).refund("u1", new BigDecimal("30"));
    }

    @Test
    void refund_equalsPaid_callsGateway() {
        when(gateway.refund("u1", PAID)).thenReturn(true);
        LegacyRefundService svc = new LegacyRefundService(gateway);

        boolean result = svc.refund("u1", PAID, PAID);

        assertTrue(result);
    }

    @ParameterizedTest
    @CsvSource({"0", "-1", "90.01"})
    void refund_invalidAmount_rejectedWithoutCallingGateway(BigDecimal amount) {
        LegacyRefundService svc = new LegacyRefundService(gateway);

        boolean result = svc.refund("u1", PAID, amount);

        assertFalse(result);
        verify(gateway, never()).refund(anyString(), any());
    }

    @Test
    void refund_gatewayFails_returnsFalse() {
        when(gateway.refund("u1", new BigDecimal("30"))).thenReturn(false);
        LegacyRefundService svc = new LegacyRefundService(gateway);

        boolean result = svc.refund("u1", PAID, new BigDecimal("30"));

        assertFalse(result);
    }
}
