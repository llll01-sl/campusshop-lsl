package campusshop;

import org.junit.jupiter.api.Test;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;

class CouponServiceTest {

    @Test
    void isUsable_validCoupon_returnsTrue() {
        // Arrange
        CouponService service = new CouponService();
        Coupon coupon = CouponFactory.valid();

        // Act
        boolean result = service.isUsable(coupon, new BigDecimal("150"));

        // Assert
        assertTrue(result);
    }

    @Test
    void isUsable_expiredCoupon_returnsFalse() {
        // Arrange
        CouponService service = new CouponService();
        Coupon coupon = CouponFactory.expiredYesterday();

        // Act
        boolean result = service.isUsable(coupon, new BigDecimal("150"));

        // Assert
        assertFalse(result);
    }

    @Test
    void isUsable_belowThreshold_returnsFalse() {
        CouponService service = new CouponService();
        Coupon coupon = CouponFactory.valid();

        boolean result = service.isUsable(coupon, new BigDecimal("99.99"));

        assertFalse(result);
    }

    @Test
    void isUsable_exactlyThreshold_returnsTrue() {
        CouponService service = new CouponService();
        Coupon coupon = CouponFactory.valid();

        boolean result = service.isUsable(coupon, new BigDecimal("100"));

        assertTrue(result);
    }
}
