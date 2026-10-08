package campusshop;

import org.junit.jupiter.api.Disabled;
import org.junit.jupiter.api.Test;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

// 第 6 周课堂练习：这个文件中的测试都有问题，请找出来。
class ProblemTest {

    private final PriceCalculator calc = new PriceCalculator();

    @Test void test1()
    { calc.calculateTotal(new BigDecimal("150"), MemberLevel.NORMAL); }

    @Test void test2()
    { assertNotNull(calc.calculateTotal(new BigDecimal("150"), MemberLevel.GOLD)); }

    @Test void test3()
    { BigDecimal t = new BigDecimal("250");
      BigDecimal exp = t.compareTo(new BigDecimal("200")) >= 0 ? t.subtract(new BigDecimal("30")) : t;
      assertEquals(0, exp.compareTo(calc.calculateTotal(t, MemberLevel.NORMAL))); }

    @Test void test4()
    { try { calc.calculateTotal(new BigDecimal("-1"), MemberLevel.NORMAL); }
      catch (Exception e) { } }

    @Disabled("flaky") @Test void test5()
    { assertEquals(0, new BigDecimal("94.50").compareTo(calc.calculateTotal(new BigDecimal("110"), MemberLevel.SILVER))); }
}
