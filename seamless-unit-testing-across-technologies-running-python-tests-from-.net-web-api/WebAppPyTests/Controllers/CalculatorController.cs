namespace WebAppPyTests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Javonet.Netcore.Sdk;

    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("adding")]
        public async Task<ActionResult<int>> GetResult([FromQuery] int a, [FromQuery] int b)
        {
            var pythonRuntime = Javonet.InMemory().Python();

            pythonRuntime.LoadLibrary("your-python-class-path");

            var calc = pythonRuntime.GetType("calculator.Calculator").Execute();
            var result = calc.InvokeInstanceMethod("add", calc, a, b).Execute();

            var value = (int)result.GetValue();

            return Ok(value);
        }
    }
}