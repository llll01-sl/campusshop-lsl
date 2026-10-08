# CampusShop

校园商城教学项目。C# 版位于 csharp/（.NET 10），Java 版位于 java/（JDK 21 + Spring Boot）。
需求文档：@docs/requirements.md

## 命令
- C# 运行全部测试：cd csharp; dotnet test
- C# 只运行一个测试类：dotnet test --filter PriceCalculatorTests
- Java 运行全部测试：cd java; .\mvnw test
- Java 只运行一个测试类：.\mvnw test -Dtest=PriceCalculatorTest
- 启动网站：C# 为 dotnet run --project csharp/src/CampusShop.Web（http://localhost:5000）；
  Java 为 cd java; .\mvnw spring-boot:run（http://localhost:8080）

## 代码规范
- 金额一律使用 decimal（C#）或 BigDecimal（Java），禁止使用 double
- 公开方法需写注释（C# 用 XML 注释，Java 用 Javadoc）

## 测试规范
- C#：xUnit；多组数据用 [Theory] + [InlineData]
- Java：JUnit 5；多组数据用 @ParameterizedTest + @CsvSource；BigDecimal 用 compareTo 比较
- 命名：方法名_场景_预期结果
- 结构：Arrange / Act / Assert，三段之间空一行
- 期望值必须根据需求计算，不得照抄代码运行结果

## 禁止事项
- 禁止修改或删除已有的测试和断言
- 禁止修改 csharp/、java/ 和 docs/ 以外的文件

## 完成任务时
- 运行全部测试，报告通过与失败的数量
