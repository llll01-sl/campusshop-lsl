package campusshop.api;

import io.restassured.http.ContentType;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.context.SpringBootTest.WebEnvironment;
import org.springframework.boot.test.web.server.LocalServerPort;

import java.util.Map;

import static io.restassured.RestAssured.given;
import static org.hamcrest.Matchers.emptyOrNullString;
import static org.hamcrest.Matchers.equalTo;
import static org.hamcrest.Matchers.not;

/**
 * API 测试示例：@SpringBootTest 在随机端口启动整个网站，不需要先 spring-boot:run。
 */
@SpringBootTest(webEnvironment = WebEnvironment.RANDOM_PORT)
class AuthApiTest {

    @LocalServerPort
    int port;

    @Test
    void login_validUser_returnsToken() {
        given()
            .port(port)
            .contentType(ContentType.JSON)
            .body(Map.of("username", "alice",
                         "password", "123456"))
        .when()
            .post("/api/auth/login")
        .then()
            .statusCode(200)
            .body("token", not(emptyOrNullString()))
            .body("expiresIn", equalTo(3600));
    }
}
