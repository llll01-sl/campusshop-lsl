package campusshop;

import org.junit.jupiter.api.Test;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

// 第 6 周课堂练习的修正版：每个测试都有明确的断言，期望值按需求手算。
class ProblemTest {

    private final PriceCalculator calc = new PriceCalculator();

    @Test
    void test1_total150_normal_returns140() {
        BigDecimal result = calc.calculateTotal(new BigDecimal("150"), MemberLevel.NORMAL);

        // 需求 2.3：150 ≥ 100，减 10 → 140.00
        assertEquals(0, new BigDecimal("140.00").compareTo(result));
    }

    @Test
    void test2_total150_gold_returns125() {
        BigDecimal result = calc.calculateTotal(new BigDecimal("150"), MemberLevel.GOLD);

        // 需求 2.2、2.3：150 × 0.9 = 135，满 100 减 10 → 125.00
        assertEquals(0, new BigDecimal("125.00").compareTo(result));
    }

    @Test
    void test3_total250_normal_returns220() {
        BigDecimal result = calc.calculateTotal(new BigDecimal("250"), MemberLevel.NORMAL);

        // 需求 2.3：250 ≥ 200，减 30 → 220.00
        assertEquals(0, new BigDecimal("220.00").compareTo(result));
    }

    @Test
    void test4_negativeTotal_throws() {
        // 需求 2.1：原价小于 0 时抛出参数越界异常
        assertThrows(IllegalArgumentException.class,
                () -> calc.calculateTotal(new BigDecimal("-1"), MemberLevel.NORMAL));
    }

    @Test
    void test5_total110_silver_returns94_50() {
        BigDecimal result = calc.calculateTotal(new BigDecimal("110"), MemberLevel.SILVER);

        // 需求 2.2、2.3：110 × 0.95 = 104.50，满 100 减 10 → 94.50
        assertEquals(0, new BigDecimal("94.50").compareTo(result));
    }
}
