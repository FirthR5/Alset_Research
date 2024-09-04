using Alset_Research.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Security.Claims;
//using Alset_Research.Services;
using Microsoft.EntityFrameworkCore;
using Alset_Research.DTO;

namespace Alset_Research.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly AlsetJournalsContext _context;

        public HomeController(ILogger<HomeController> logger, AlsetJournalsContext context )
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
            var my_user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!my_user.IsNullOrEmpty()) return View();
            else return RedirectToAction("Login", "Home");
		}
        #region login
        public IActionResult Login()
		{
            var my_user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!my_user.IsNullOrEmpty()) return View();
            else return RedirectToAction("Login", "Home");
		}
		[HttpPost]
        public async Task<IActionResult> Login(LoginDTO data)
        {
            //if (!ModelState.IsValid) { TempData["Error"] = "Invalid format"; return View("Login", loginDTO); }
           var CheckUser = await _context.Users.FirstOrDefaultAsync(e => e.Email == data.email);
            int UserId = CheckUser != null ? CheckUser.Id : 0;

            if(UserId ==0 )
            {
                ViewBag.ErrorMessage = "Incorrect Credentials.";

                return View("Login");
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
            };
            claims.Add(new Claim("Name", $"{CheckUser.FirstName} {CheckUser.LastName}"));
            ClaimsIdentity claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
             AuthenticationProperties properties = new AuthenticationProperties()
            { AllowRefresh = true,/*IsPersistent =false; */};

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity),
                properties);

            return RedirectToAction("Index", "Home");
        }

		[HttpPost]
        public async Task<IActionResult> LogOut()

        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
		}
        #endregion


        #region UploadJournals
        
        public IActionResult UploadJournals() { 
			return View();
		}

		[HttpPost]
		public IActionResult UploadJournals(string email) { 
			return View();
		}

        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
