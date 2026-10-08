package campusshop.e2e;

import com.microsoft.playwright.BrowserContext;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.Tracing;
import com.microsoft.playwright.junit.UsePlaywright;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Tag;
import org.junit.jupiter.api.TestInfo;

import java.nio.file.Paths;

import static com.microsoft.playwright.assertions.PlaywrightAssertions.assertThat;

/**
 * E2E 测试的基类：提供网站地址、登录步骤，并为每个测试记录 Trace（保存在 java/traces/）。
 * 运行前要先启动网站（.\mvnw spring-boot:run），再在另一个终端运行 .\mvnw test -Pe2e。
 */
@Tag("e2e")
@UsePlaywright
abstract class E2ETest {

    static final String BASE_URL = "http://localhost:8080";

    @BeforeEach
    void startTrace(BrowserContext context) {
        context.tracing().start(new Tracing.StartOptions()
            .setScreenshots(true).setSnapshots(true)
            .setSources(true));
    }

    @AfterEach
    void stopTrace(BrowserContext context, TestInfo info) {
        context.tracing().stop(new Tracing.StopOptions()
            .setPath(Paths.get("traces",
                info.getTestMethod().orElseThrow().getName() + ".zip")));
    }

    /** 用演示账号登录（在密码框按回车提交），并等待欢迎信息出现。 */
    static void loginAs(Page page, String user) {
        page.navigate(BASE_URL);
        page.getByLabel("用户名").fill(user);
        page.getByLabel("密码").fill("123456");
        page.getByLabel("密码").press("Enter");
        assertThat(page.getByText("欢迎，" + user)).isVisible();
    }
}
