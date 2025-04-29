# Introduction

Modern applications often consist of multiple components written in different technologies. Companies using microservices, multi-layered systems, or external integrations face the challenge of testing business logic spread across various environments.

Standard approaches require either running unit tests separately for each technology or simulating application behavior by mocking its functionality. However, such methods do not always provide a complete picture of the system's actual operation.

With Javonet, it is possible to directly invoke classes from one technology within another and test them as an integral part of the system. This means that unit tests executed for the API can verify the actual behavior of individual components instead of relying on mocked responses.

This solution has many practical applications:

Enables full integration testing – instead of mocking external systems, they can be executed and their behavior can be verified.
Increases test coverage and facilitates diagnostics – by allowing real interactions between system components.
In this article, we will discuss how to set up unit tests in .NET Web API that execute Python classes using Javonet.

[TOC]

## Activating Javonet

If you haven't installed Javonet yet, here are the exact instructions for [.NET](https://www.javonet.com/guides/v2/python/net-dll/getting-started/getting-started-dotnet).

## Sample Python code
In the first step, let's create and save a very simple Python class:

```python=
class Calculator:
    __type__ = "Calculator"

    def add(self, a: int, b: int):
        return a + b
```

## .NET Web API
>[!NOTE]
>Due to some technical limitations of Javonet we have to use classic `Main()` structure of `Program.cs`

Next, we need to create a .NET Web API, install the **Javonet.Netcore.Sdk** NuGet package, and activate **Javonet**. Our `Program.cs` file should look like this:

```csharp=
namespace WebAppPyTests
{
    using Javonet.Netcore.Sdk;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            Javonet.Activate("your-license-key"); // <= activating Javonet!

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
```
### API Code

Now, we have to add some controller:
```csharp=
namespace WebAppPyTests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Javonet.Netcore.Sdk;

    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        [HttpGet("adding")]
        public async Task<ActionResult<int>> GetResult([FromQuery] int a, [FromQuery] int b)
        {
            var pythonRuntime = Javonet.InMemory().Python();

            pythonRuntime.LoadLibrary(@"C:\coding\python\Calculator");
            var calc = pythonRuntime.GetType("calculator.Calculator").Execute();
            var result = calc.InvokeInstanceMethod("add", calc, a, b).Execute();
            var value = (int)result.GetValue();

            return Ok(value);
        }
    }
}
```
At this stage, we can test our endpoint by calling `/calculator/adding?a=5&b=7`. We should get `12` as a result.

### Unit tests

Now, we can create another project in our solution – an xUnit Test Project – and add our API project as a reference. Our test class may look like this (remember to activate Javonet in the constructor):

```csharp=
namespace WebAppPyTests.Tests
{
    using WebAppPyTests.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Javonet.Netcore.Sdk;
    using System.Threading.Tasks;

    public class JavonetTests
    {
        private readonly CalculatorController _sut;

        public JavonetTests()
        {
            Javonet.Activate("your-license-key");

            _sut = new CalculatorController();
        }

        [Fact]
        public async Task GetResult_ReturnsOkResult()
        {
            // Act
            var result = await _sut.GetResult(2, 4);

            // Assert
            Assert.IsAssignableFrom<ActionResult<int>>(result);
            Assert.NotNull(result);
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task GetResult_ReturnsCorrectResult()
        {
            // Act
            var result = await _sut.GetResult(2, 4);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualValue = Assert.IsType<int>(okResult.Value);

            // Assert
            Assert.Equal(6, actualValue);
        }
    }
}

```
## Conclusion

In this article, we explored how to integrate and test Python classes directly from a .NET Web API using Javonet. By leveraging this approach, developers can execute Python logic as a native part of their .NET system, eliminating the need for external service calls or extensive mocking.
This solution enhances test coverage, enables full integration testing, and simplifies debugging in multi-language environments. By incorporating Javonet, developers can ensure smoother interoperability between .NET and Python while maintaining high testing standards.