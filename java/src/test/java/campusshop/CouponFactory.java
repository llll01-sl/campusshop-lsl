package campusshop;

import java.math.BigDecimal;
import java.time.LocalDate;

/**
 * 测试用的优惠券：满 100 减 20，日期相对于“今天”计算。
 */
public final class CouponFactory {

    private CouponFactory() {
    }

    public static Coupon expiresInDays(int days, BigDecimal threshold, boolean used) {
        return new Coupon("TEST", threshold, new BigDecimal("20"), LocalDate.now().plusDays(days), used);
    }

    public static Coupon expiresInDays(int days) {
        return expiresInDays(days, new BigDecimal("100"), false);
    }

    /** 一个月后过期。 */
    public static Coupon valid() {
        return expiresInDays(30);
    }

    /** 今天是有效期最后一天。 */
    public static Coupon lastDayToday() {
        return expiresInDays(0);
    }

    /** 昨天已过期。 */
    public static Coupon expiredYesterday() {
        return expiresInDays(-1);
    }
}
