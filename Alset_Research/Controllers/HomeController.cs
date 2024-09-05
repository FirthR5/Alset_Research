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
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Alset_Research.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly AlsetJournalsContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public HomeController(ILogger<HomeController> logger, AlsetJournalsContext context, IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int id = int.Parse(userId);

            var query = @"
                    DECLARE @UserId INT = @id;
                    SELECT U.Id AS ResearcherId, U.FirstName, U.LastName,
                           CASE 
                               WHEN F.FollowerId IS NOT NULL THEN 1
                               ELSE 0
                           END AS FollowStatus
                    FROM Users U
                    LEFT JOIN Followers F ON U.Id = F.ResearcherId AND F.FollowerId = @UserId
                    WHERE U.Id <> @UserId;
                ";
            var userIdParam = new SqlParameter("@id", id);


            List<ResearchDTO> result =  _context.ResearchDTOs
                .FromSqlRaw(query, userIdParam)
                .ToList();


            return View(result);
        }
        #region login
        public IActionResult Login()
		{
            var my_user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (!my_user.IsNullOrEmpty()) return View();
            //else return RedirectToAction("Login", "Home");
            return View();
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
        [Authorize]
		public async Task<IActionResult> UploadJournals(UploadJournalDTO journalFile) {

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var PdfFileName=await UploadJournal(journalFile.JournalPDF);
            Journal journal = new Journal()
            {
                Title=journalFile.Title,
                Description=journalFile.Description,
                PublicationDate=DateOnly.FromDateTime(DateTime.Today),
                Pdffile = PdfFileName,
                UserId = userId
            };

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();

			return View();
		    }
		private async Task<string> UploadJournal( IFormFile file)
		{
            string folderPath = "journals/";
            string filename = Guid.NewGuid().ToString()  + "_" + file.FileName;
            folderPath += filename;

			string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

			await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

			return filename;
		}

		#endregion

		public IActionResult Journals()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int id = int.Parse(userId);

            var query = @"
                SELECT J.Id, J.Title, J.Description, J.PublicationDate, J.PDFFile,
                       U.FirstName AS ResearcherFirstName, U.LastName AS ResearcherLastName
                FROM Journals J
                JOIN Followers F ON J.UserId = F.ResearcherId
                JOIN Users U ON J.UserId = U.Id
                WHERE F.FollowerId = @id;
                                    ";
            var userIdParam = new SqlParameter("@id", id);

            List<JournalDTO> result =  _context.JournalsDTOs
                .FromSqlRaw(query, userIdParam)
                .ToList();


            return View(result);
        }

        public async Task<IActionResult> FollowResearch(int IdResearch)
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int my_id = int.Parse(userId);

            Follower newFollower = new Follower()
            {
                ResearcherId = IdResearch,
                FollowerId = my_id,
            };

            _context.Followers.Add(newFollower);
            _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
