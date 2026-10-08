package campusshop;

import java.math.BigDecimal;

/**
 * 退款。见 docs/requirements.md 4.11。
 * 支付网关通过构造函数注入：生产环境传入 AlipayGateway，测试中传入测试替身。
 */
public class LegacyRefundService {

    private final PaymentGateway gateway;

    public LegacyRefundService(PaymentGateway gateway) {
        this.gateway = gateway;
    }

    /**
     * 为用户退款。paid 为订单实付金额，amount 为本次退款金额。
     * 退款成功返回 true。
     */
    public boolean refund(String userId, BigDecimal paid, BigDecimal amount) {
        if (amount.signum() <= 0 || amount.compareTo(paid) > 0)
            return false;

        return gateway.refund(userId, amount);
    }
}
