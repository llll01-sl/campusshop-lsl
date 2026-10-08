package campusshop;

import java.math.BigDecimal;

/** 支付网关：扣款、退款成功返回 true。 */
public interface PaymentGateway {
    boolean charge(String userId, BigDecimal amount);

    boolean refund(String userId, BigDecimal amount);
}
