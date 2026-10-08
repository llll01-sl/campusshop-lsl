package campusshop;

import net.jqwik.api.ForAll;
import net.jqwik.api.Property;
import net.jqwik.api.constraints.IntRange;

import java.math.BigDecimal;

/**
 * 属性测试：jqwik 随机生成 1000 组输入，检查需求保证的规律是否总是成立。
 */
class PriceCalculatorPropertyTest {

    @Property
    boolean totalNeverNegativeNeverExceedsOriginal(
            @ForAll @IntRange(min = 0, max = 10_000_000) int cents,
            @ForAll MemberLevel level) {
        // 把随机整数看作“分”：0 ~ 100000 元（需求 2.1 的取值范围）
        BigDecimal total = BigDecimal.valueOf(cents, 2);

        BigDecimal result = new PriceCalculator()
                .calculateTotal(total, level);

        return result.signum() >= 0
                && result.compareTo(total) <= 0;
    }

    @Property
    boolean applyDiscountReducesBy0Or10Or30(@ForAll @IntRange(min = 0, max = 10_000_000) int cents) {
        BigDecimal total = BigDecimal.valueOf(cents, 2);

        BigDecimal reduction = total.subtract(new PriceCalculator().applyDiscount(total));

        // 需求 2.3：满减只有 0、10、30 三档
        return reduction.compareTo(BigDecimal.ZERO) == 0
                || reduction.compareTo(BigDecimal.TEN) == 0
                || reduction.compareTo(new BigDecimal("30")) == 0;
    }

    // 课堂发现：“金卡永远不比银卡贵”并不成立（例如原价 110 元：金卡 99.00，银卡 94.50）。
    // 这是需求的漏洞，不是代码缺陷，已记录在 requirements.md 2.6，等待产品确认，因此这里不写成测试。
}
