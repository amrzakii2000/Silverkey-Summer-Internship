using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecurityDemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecurityDemo.Controllers
{
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

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]         //This view requires authorization
        public IActionResult Secured()
        {
            return View();
        }

        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [HttpGet("login")]             //To route as to login page
        public IActionResult Login(string returnUrl)        //ReturnUrl is found in the url of the login page
        {
            ViewData["ReturnUrl"] = returnUrl;     //Allows me to access data in the view 
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> ValidateUser(string userName, string password, string returnUrl)     //Submitting user credentials is followed by validation. Normally, this should be validated trhough a database.
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (userName == "Amr" && password == "learning")
            {
                var claims = new List<Claim>();   //Claims are properties that describe a user
                claims.Add(new Claim("username", userName));
                claims.Add(new Claim("Name", "Amr Zaki"));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userName));
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync(claimPrincipal);
                return Redirect(returnUrl);

                //1-Principal is used to make a token using CookieAuthenticationDefault.
                //2-This token will be stored inside of a cookie.
                //3-With every request that requires validation, the token helps the server to recognize you and the server is responsible of making sure that the cookie is valid and not expired.

            }
            TempData["Error"] = "Invalid login credintials"; //This time we are using TempData
            return View("login");
        }

        [Authorize(Roles ="Admin")]  //Authorization needs specific user roles
        public async Task<IActionResult> LogOut()
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
