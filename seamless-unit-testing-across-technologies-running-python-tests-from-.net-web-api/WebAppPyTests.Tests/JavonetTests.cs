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
            Javonet.Activate("n9B5-Km7g-Pp69-j9FE-e9A5");

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
