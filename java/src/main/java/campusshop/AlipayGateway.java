package campusshop;

import java.math.BigDecimal;

/**
 * 支付宝网关。真实环境中每次调用都要访问支付宝服务器；
 * 教学项目中没有真实的支付接口，调用时直接抛出异常。
 */
public class AlipayGateway implements PaymentGateway {

    @Override
    public boolean charge(String userId, BigDecimal amount) {
        throw new IllegalStateException("无法连接支付宝服务器");
    }

    @Override
    public boolean refund(String userId, BigDecimal amount) {
        throw new IllegalStateException("无法连接支付宝服务器");
    }
}
