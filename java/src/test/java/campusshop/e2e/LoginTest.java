package campusshop.e2e;

import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;
import org.junit.jupiter.api.Test;

import static com.microsoft.playwright.assertions.PlaywrightAssertions.assertThat;

class LoginTest extends E2ETest {

    @Test
    void login_validUser_showsWelcome(Page page) {
        // Arrange
        page.navigate(BASE_URL);
        // Act
        page.getByLabel("用户名").fill("alice");
        page.getByLabel("密码").fill("123456");
        page.getByRole(AriaRole.BUTTON,
            new Page.GetByRoleOptions().setName("登录"))
            .click();
        // Assert
        assertThat(page.getByText("欢迎，alice"))
            .isVisible();
    }

    @Test
    void login_wrongPassword_showsError(Page page) {
        page.navigate(BASE_URL);

        page.getByLabel("用户名").fill("alice");
        page.getByLabel("密码").fill("wrong");
        page.getByRole(AriaRole.BUTTON,
            new Page.GetByRoleOptions().setName("登录"))
            .click();

        assertThat(page.getByTestId("error-msg"))
            .hasText("用户名或密码错误");
    }
}
