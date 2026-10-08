package campusshop;

import java.math.BigDecimal;
import java.math.RoundingMode;

/**
 * 订单计价。规则见 docs/requirements.md 第 2 节。
 */
public class PriceCalculator {

    /**
     * 满减：满 200 元减 30 元；满 100 元减 10 元。取最高一档，不叠加。
     */
    public BigDecimal applyDiscount(BigDecimal total) {
        if (total.compareTo(new BigDecimal("200")) >= 0)
            return total.subtract(new BigDecimal("30"));
        if (total.compareTo(new BigDecimal("100")) >= 0)
            return total.subtract(BigDecimal.TEN);
        return total;
    }

    /**
     * 计算应付金额：先按会员等级打折，再按折后金额满减。
     *
     * @throws IllegalArgumentException total 小于 0 或大于 100000
     */
    public BigDecimal calculateTotal(BigDecimal total, MemberLevel level) {
        if (total.signum() < 0 || total.compareTo(new BigDecimal("100000")) > 0)
            throw new IllegalArgumentException("total 超出范围：" + total);

        BigDecimal discounted = BigDecimal.valueOf(Math.round(total.doubleValue() * rate(level) * 100) / 100.0)
                .setScale(2, RoundingMode.HALF_UP);
        if (discounted.compareTo(new BigDecimal("200")) >= 0)
            return discounted.subtract(new BigDecimal("30"));
        if (discounted.compareTo(new BigDecimal("100")) >= 0)
            return discounted.subtract(BigDecimal.TEN);
        return discounted;
    }

    private static double rate(MemberLevel level) {
        return switch (level) {
            case SILVER -> 0.95;
            case GOLD -> 0.9;
            default -> 1.0;
        };
    }
}
