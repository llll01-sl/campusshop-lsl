package campusshop;

import java.math.BigDecimal;

/**
 * 退款（旧代码）。见 docs/requirements.md 4.11。
 */
public class LegacyRefundService {

    /**
     * 为用户退款。paid 为订单实付金额，amount 为本次退款金额。
     * 退款成功返回 true。
     */
    public boolean refund(String userId, BigDecimal paid, BigDecimal amount) {
        if (amount.signum() <= 0 || amount.compareTo(paid) > 0)
            return false;

        AlipayGateway gateway = new AlipayGateway();
        return gateway.refund(userId, amount);
    }
}
