using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Controllers
{
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            return View();
        }

        //[Authorize]
        public async Task<IActionResult> Secured()
        {
            var token = await HttpContext.GetTokenAsync("id_token");
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = "")
        {
            if(username == "Bob" && password == "123")
            {
                //var claims = new List<Claim>();
                //claims.Add(new Claim(ClaimTypes.Name, username));
                //claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                //var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                //await HttpContext.SignInAsync(claimPrincipal);
                //return LocalRedirect(returnUrl);

                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, username)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(principal);
                if(returnUrl == "")
                {
                    return RedirectToAction("Index");
                } else return LocalRedirect(returnUrl);
            }

            return View("login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
