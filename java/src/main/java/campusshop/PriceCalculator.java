package campusshop;

import java.math.BigDecimal;

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
        if (total.compareTo(new BigDecimal("100")) > 0)
            return total.subtract(BigDecimal.TEN);
        return total;
    }
}
