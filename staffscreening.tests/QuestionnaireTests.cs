using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffScreening.Controllers;
using StaffScreening.Data;
using StaffScreening.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Moq;

namespace staffscreening.tests
{
    [TestClass]
    public class QuestionnaireTests
    {
        private HomeController _controller;
        private ApplicationDbContext _context;
        private Mock<ILogger<HomeController>> _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestQuestionnaireDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _context);

        }

        [TestMethod]
        public void Questionnaire_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Questionnaire();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Questionnaire_Post_Incomplete_ReturnsViewWithError()
        {
            // Arrange
            var incompleteModel = new ScreeningQuestionnaire { Question1 = true };

            // Act
            var result = _controller.Questionnaire(incompleteModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
            var modelState = _controller.ModelState[string.Empty];
            Assert.IsTrue(modelState != null && modelState.Errors.Count > 0);
        }

        [TestMethod]
        public void Questionnaire_Post_InvalidModelState_ReturnsViewWithModel()
        {
            // Arrange
            var model = new ScreeningQuestionnaire();
            _controller.ModelState.AddModelError("Question1", "An error occurred");

            // Act
            var result = _controller.Questionnaire(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual(model, result.Model);
        }
    }
}
