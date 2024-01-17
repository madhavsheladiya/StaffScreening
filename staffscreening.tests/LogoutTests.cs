using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffScreening.Controllers;
using StaffScreening.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Threading.Tasks;

namespace staffscreening.tests
{
    [TestClass]
    public class LogoutTests
    {
        private HomeController _controller;
        private ApplicationDbContext _context;
        private Mock<ILogger<HomeController>> _loggerMock;
        private Mock<HttpContext> _httpContextMock;
        private Mock<IAuthenticationService> _authMock;
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<IUrlHelper> _urlHelperMock;
        private Mock<IUrlHelperFactory> _urlHelperFactoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestLogoutDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _context);

            _httpContextMock = new Mock<HttpContext>();
            _authMock = new Mock<IAuthenticationService>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _urlHelperFactoryMock = new Mock<IUrlHelperFactory>();

            // Set up the HttpContext to use the mocked service provider
            _httpContextMock.Setup(x => x.RequestServices).Returns(_serviceProviderMock.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

            // Mock the authentication service
            _authMock.Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                     .Returns(Task.CompletedTask);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(_authMock.Object);

            // Mock the UrlHelperFactory and UrlHelper
            _urlHelperFactoryMock.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(_urlHelperMock.Object);
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IUrlHelperFactory))).Returns(_urlHelperFactoryMock.Object);

            // Assign the mocked UrlHelper to the controller
            _controller.Url = _urlHelperMock.Object;
        }

        [TestMethod]
        public async Task Logout_ExecutesSignOut()
        {
            // Act
            await _controller.Logout();

            // Assert
            _authMock.Verify(x => x.SignOutAsync(_httpContextMock.Object, "CookieAuth", It.IsAny<AuthenticationProperties>()), Times.Once());
        }
    }
}