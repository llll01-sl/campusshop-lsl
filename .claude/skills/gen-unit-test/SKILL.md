---
name: gen-unit-test
description: 为 CampusShop 中指定的类或方法生成符合项目规范的单元测试并运行。当用户要求“写测试”“补测试”“生成单元测试”时使用。
---

# 生成单元测试

1. 阅读被测代码和 docs/requirements.md 中的相关需求。
2. 用表格列出等价类、边界值和异常场景，等待用户确认：
   | 场景 | 输入 | 期望 | 用例类型 | 需求条款 |
3. 按项目规范生成测试：
   - C#：xUnit，命名 `方法名_场景_预期结果`，多组数据用 [Theory] + [InlineData]。
   - Java：JUnit 5，命名同上，多组数据用 @ParameterizedTest + @CsvSource。
   - 结构：Arrange / Act / Assert，三段之间空一行。
   - 期望值根据需求计算，不得照抄被测代码的运行结果。
4. 运行测试（C#：`cd csharp; dotnet test`；Java：`cd java; .\mvnw test`）。
5. 如果测试失败：判断是测试写错还是发现了缺陷。**不得修改断言来让测试通过**，发现缺陷时向用户报告。
6. 输出总结：新增测试数、通过/失败数、发现的疑似缺陷。
