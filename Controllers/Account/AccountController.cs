using Microsoft.AspNetCore.Mvc;
using Celsia.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Celsia.ViewModels;
using Microsoft.AspNetCore.Authentication.Google;
using System.Text.Json;

namespace Celsia.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AccountController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home"); // Ajusta según la ruta de tu vista principal
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCreateViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Error en los campos.";
                return View();
            }

            try
            {
                var user = await _authRepository.Register(userVM);

                if (!string.IsNullOrEmpty(user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }

                ViewData["Message"] = "No se pudo registrar el usuario, error fatal.";
            }
            catch (Exception ex)
            {
                // Log the exception message
                ViewData["Message"] = "Error al registrar el usuario: " + ex.Message;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard"); // Ajusta según la ruta de tu vista principal
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userVM)
        {

            var user = await _authRepository.Login(userVM);

            if (user == null)
            {
                ViewData["Message"] = "Correo o contraseña incorrectos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Dashboard"); // Ajusta según la ruta de tu vista principal
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();  // Limpia la sesión completamente
            return RedirectToAction("Login", "Account");
        }


    }
}
