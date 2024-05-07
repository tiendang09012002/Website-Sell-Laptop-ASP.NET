using BTL.Models;
using BTL.Models.DTO;
using BTL.Repository;
using BTL.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
		private readonly IUserAuthenticationService _authService;
		public AdminController(IUserAuthenticationService authService)
		{
			this._authService = authService;
		}
		public IActionResult Display()
        {
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			return View();
        }
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await this._authService.LogoutAsync();
			return RedirectToAction("Login","Account");
		}
	}
}