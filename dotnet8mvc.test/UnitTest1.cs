using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnet8mvctest.Controllers;
using dotnet8mvctest.Models;
using Xunit;

namespace dotnet8mvc.test
{
    public class HomeControllerTests
    {
        private static ILogger<HomeController> GetLogger() => new TestLogger<HomeController>();

        [Fact]
        public void Index_Returns_ViewResult()
        {
            // Arrange
            var controller = new HomeController(GetLogger());

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public void Privacy_Returns_ViewResult()
        {
            // Arrange
            var controller = new HomeController(GetLogger());

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_Returns_ViewResult_With_RequestId_From_HttpContext()
        {
            // Arrange
            var controller = new HomeController(GetLogger())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { TraceIdentifier = "trace-123" }
                }
            };

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Equal("trace-123", model.RequestId);
            Assert.Equal(!string.IsNullOrEmpty(model.RequestId), model.ShowRequestId);
        }
    }

    // Minimal test logger to avoid adding third-party dependencies.
    internal class TestLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
        public bool IsEnabled(LogLevel logLevel) => false;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception exception, System.Func<TState, System.Exception, string> formatter) { }
    }

    internal sealed class NullScope : System.IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        private NullScope() { }
        public void Dispose() { }
    }
}