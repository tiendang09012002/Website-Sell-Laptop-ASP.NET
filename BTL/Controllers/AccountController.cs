using BTL.Models;
using BTL.Models.DTO;
using BTL.Repository;
using BTL.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        public AccountController(IUserAuthenticationService authService)
        {
            this._authService = authService;
        }


        public IActionResult Login()
        {
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "user";
            var result = await this._authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return Redirect("/account/login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin()
        {
            RegistrationModel model = new RegistrationModel
            {
                Username = "admin",
                Email = "admin@gmail.com",
                FirstName = "admin",
                LastName = "admin",
                Password = "Admin@12345"
            };
            model.Role = "admin";
            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }

    }
}