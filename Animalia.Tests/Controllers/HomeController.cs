using Moq;
using Microsoft.Extensions.Logging;
using Animalia.Controllers;  // Asegura que tomas el controlador correcto

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _loggerMock;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _loggerMock = new Mock<ILogger<HomeController>>();
        _controller = new HomeController(_loggerMock.Object);
    }
}
