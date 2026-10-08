package campusshop;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

class PriceCalculatorTest {

    @Test
    void applyDiscount_totalIs0_returns0() {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(new BigDecimal("0"));

        assertEquals(0, new BigDecimal("0").compareTo(result));
    }

    @Test
    void applyDiscount_totalIs50_returns50() {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(new BigDecimal("50"));

        assertEquals(0, new BigDecimal("50").compareTo(result));
    }

    @Test
    void applyDiscount_totalIs99_returns99() {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(new BigDecimal("99"));

        assertEquals(0, new BigDecimal("99").compareTo(result));
    }

    @Test
    void applyDiscount_totalIs120_returns110() {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(new BigDecimal("120"));

        assertEquals(0, new BigDecimal("110").compareTo(result));
    }

    @Test
    void applyDiscount_totalIs150_returns140() {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(new BigDecimal("150"));

        assertEquals(0, new BigDecimal("140").compareTo(result));
    }

    @Test
    void applyDiscount_totalIs180_returns170() {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(new BigDecimal("180"));

        assertEquals(0, new BigDecimal("170").compareTo(result));
    }

    @ParameterizedTest
    @CsvSource({
            "200,    170",    // 正好 200：减 30
            "250,    220",
            "199.99, 189.99"  // 不足 200：仍按满 100 减 10
    })
    void applyDiscount_tier200_minus30(BigDecimal total, BigDecimal expected) {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(total);

        assertEquals(0, expected.compareTo(result));
    }

    // ---- calculateTotal：需求第 2 节 ----

    @ParameterizedTest
    @CsvSource({
            "99.99,  NORMAL, 99.99",    // 边界 100 左
            "100,    NORMAL, 90.00",    // 边界 100
            "200,    NORMAL, 170.00",   // 边界 200
            "200,    GOLD,   170.00",   // 交互：200 × 0.9 = 180 → 减 10
            "110,    SILVER, 94.50",    // 交互：110 × 0.95 = 104.5 → 减 10
            "105,    SILVER, 99.75"     // 交互：105 × 0.95 = 99.75，跌破门槛
    })
    void calculateTotal_validInput_returnsExpected(BigDecimal total, MemberLevel level, BigDecimal expected) {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.calculateTotal(total, level);

        assertEquals(0, expected.compareTo(result), () -> "expected " + expected + " but was " + result);
    }

    @ParameterizedTest
    @CsvSource({"-0.01", "100000.01"})
    void calculateTotal_outOfRange_throws(BigDecimal total) {
        PriceCalculator calc = new PriceCalculator();

        assertThrows(IllegalArgumentException.class,
                () -> calc.calculateTotal(total, MemberLevel.NORMAL));
    }

    @Test
    void applyDiscount_totalIs100_returns90() {
        PriceCalculator calc = new PriceCalculator();

        BigDecimal result = calc.applyDiscount(new BigDecimal("100"));

        assertEquals(0, new BigDecimal("90").compareTo(result));
    }
}
