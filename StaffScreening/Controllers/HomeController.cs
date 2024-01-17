using Microsoft.AspNetCore.Mvc;
using StaffScreening.Data;
using StaffScreening.Models;
using System.Diagnostics;
using BCrypt.Net;
using StaffScreening.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace StaffScreening.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User userModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == userModel.Email);
                if (existingUser != null)
                {
                    // User already exists
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(userModel);
                }

                // Encrypt the password
                userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);

                // Add user to the database
                _context.Users.Add(userModel);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            // If we got this far, something failed, redisplay form
            return View(userModel);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) // Storing the user ID
            };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);

                    return RedirectToAction("Questionnaire");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Questionnaire()
        {
            return View(new ScreeningQuestionnaire());
        }

        [HttpPost]
        public ActionResult Questionnaire(ScreeningQuestionnaire model)
        {
            // Check if all questions have been answered
            if (model.Question1 == null || model.Question2 == null || model.Question3 == null)
            {
                ModelState.AddModelError(string.Empty, "All questions must be answered.");
            }

            if (ModelState.IsValid)
            {
                // Retrieve the user's ID from the claims
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    // Handle the case where the user ID claim is not set
                    ModelState.AddModelError(string.Empty, "There was an error processing your questionnaire. Please try again.");
                    return View(model);
                }

                // Convert the claim value to int before assigning
                if (int.TryParse(userIdClaim.Value, out int userId))
                {
                    model.UserId = userId;
                }
                else
                {
                    // Handle the case where the user ID claim cannot be converted to int
                    ModelState.AddModelError(string.Empty, "There was an error processing your questionnaire. Please try again.");
                    return View(model);
                }

                // Set the result based on the answered questions
                model.DateCompleted = DateTime.UtcNow;
                model.PassedScreening = !(model.Question1!.Value || model.Question2!.Value || model.Question3!.Value);

                _context.Screenings.Add(model);
                _context.SaveChanges(); // Save the questionnaire response along with the result to the database

                // Redirect to the outcome page with the result of the screening
                return RedirectToAction("ScreeningOutcome", new { passed = model.PassedScreening });
            }

            // If we got this far, something failed, redisplay form with validation errors
            return View(model);
        }



        public ActionResult ScreeningOutcome(bool passed)
        {
            var outcomeViewModel = new ScreeningOutcomeViewModel
            {
                Passed = passed,
                Message = passed ? "You have passed the screening." : "You have failed the screening."
            };

            return View(outcomeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }


    }
}