using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffScreening.Controllers;
using StaffScreening.Data;
using StaffScreening.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Moq;

namespace staffscreening.tests
{
    [TestClass]
    public class RegisterTests
    {
        private HomeController _controller;
        private ApplicationDbContext _context;
        private Mock<ILogger<HomeController>> _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _context);
        }

        [TestMethod]
        public void Register_ReturnsViewResult()
        {
            // Act
            var result = _controller.Register();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Register_ValidUser_ReturnsRedirectToLogin()
        {
            // Arrange
            var newUser = new User { Email = "new@example.com", Password = "password" };

            // Act
            var result = _controller.Register(newUser) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ActionName);

            // Verify that the user was added to the in-memory database
            var addedUser = _context.Users.FirstOrDefault(u => u.Email == newUser.Email);
            Assert.IsNotNull(addedUser);
        }

        [TestMethod]
        public void Register_ExistingEmail_ReturnsViewWithError()
        {
            // Arrange
            var existingUser = new User { Email = "existing@example.com", Password = "password" };
            _context.Users.Add(existingUser);
            _context.SaveChanges();

            var newUser = new User { Email = "existing@example.com", Password = "newpassword" };

            // Act
            var result = _controller.Register(newUser) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var modelState = _controller.ModelState["Email"];
            Assert.IsTrue(modelState != null && modelState.Errors.Count > 0);
            Assert.AreEqual("Email already exists", modelState.Errors.First().ErrorMessage);
        }
        [TestMethod]
        public void Register_InvalidModelState_ReturnsViewWithModel()
        {
            // Arrange
            var newUser = new User { Email = "new@example.com", Password = "password" };
            _controller.ModelState.AddModelError("Email", "An error occurred");

            // Act
            var result = _controller.Register(newUser) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual(newUser, result.Model);
        }
    }
}
