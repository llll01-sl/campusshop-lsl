package campusshop;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;

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
}
