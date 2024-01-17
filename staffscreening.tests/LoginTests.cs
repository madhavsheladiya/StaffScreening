using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffScreening.Controllers;
using StaffScreening.Data;
using StaffScreening.Models;
using StaffScreening.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;

namespace staffscreening.tests
{
    [TestClass]
    public class LoginTests
    {
        private HomeController _controller;
        private ApplicationDbContext _context;
        private Mock<ILogger<HomeController>> _loggerMock;

        [TestInitialize]
        public async Task TestInitializeAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestLoginDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _context);

            var httpContext = new Mock<HttpContext>();

            // Seed the database with one user
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password");
            var user = new User { Email = "user@example.com", Password = hashedPassword };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public void Login_ReturnsViewResultOnGet()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Login_InvalidPassword_ReturnsViewWithModelError()
        {
            // Arrange
            var model = new LoginViewModel { Email = "user@example.com", Password = "incorrectpassword" };

            // Act
            var result = await _controller.Login(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.ModelState[""].Errors.Count > 0);
            Assert.AreEqual("Invalid login attempt.", _controller.ModelState[""].Errors.First().ErrorMessage);
        }

        [TestMethod]
        public async Task Login_NonExistentUser_ReturnsViewWithModelError()
        {
            // Arrange
            var model = new LoginViewModel { Email = "nonexistent@example.com", Password = "password" };

            // Act
            var result = await _controller.Login(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.ModelState[""].Errors.Count > 0);
            Assert.AreEqual("Invalid login attempt.", _controller.ModelState[""].Errors.First().ErrorMessage);
        }
    }
}
