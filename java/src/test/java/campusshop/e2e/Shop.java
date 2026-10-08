package campusshop.e2e;

import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

import static com.microsoft.playwright.assertions.PlaywrightAssertions.assertThat;

/** E2E 测试的公共步骤：网站地址和登录。 */
final class Shop {

    static final String BASE_URL = "http://localhost:8080";

    private Shop() {
    }

    /** 用演示账号登录，并等待欢迎信息出现。 */
    static void loginAs(Page page, String user) {
        page.navigate(BASE_URL);
        page.getByLabel("用户名").fill(user);
        page.getByLabel("密码").fill("123456");
        page.getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("登录")).click();
        assertThat(page.getByText("欢迎，" + user)).isVisible();
    }
}
