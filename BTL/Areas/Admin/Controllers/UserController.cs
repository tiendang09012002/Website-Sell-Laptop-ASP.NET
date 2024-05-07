using BTL.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BTL.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var allUsers = await _userManager.Users.OrderByDescending(p => p.Id).ToListAsync();

			// Lọc ra những người dùng không có vai trò là admin
			var users = allUsers.Where(x=>x.UserName != "admin");

			return View(users);
		}
		[HttpPost]
		public async Task<IActionResult> ChangeUserStatus(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			// Toggle lockout status
			user.LockoutEnabled = !user.LockoutEnabled;
			if (user.LockoutEnabled)
			{
				await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue); // Chặn vĩnh viễn
			}
			else
			{
				await _userManager.SetLockoutEndDateAsync(user, null); // Bỏ chặn
			}

			await _userManager.UpdateAsync(user);

			return RedirectToAction(nameof(Index));
		}

	}
}
