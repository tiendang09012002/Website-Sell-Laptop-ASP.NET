using BTL.Models.ViewModels;
using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext _Context)
		{
			_dataContext = _Context;
		}
		public IActionResult Index()
		{
			
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			CartItemViewModel cartVM = new()
			{
				CartItems = Cartitems,
				GrandTotal = Cartitems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}
		public IActionResult Checkout()
		{
			return View();
		}
		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItem.Quantity += 1;
			}

			HttpContext.Session.SetJson("Cart", cart);

			TempData["success"] = "Add Item to cart Successfully";

			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");


			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["success"] = "Decrease Item quantity to cart Successfully";

			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");


			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["success"] = "Increase Item quantity to cart Successfully";

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			cart.RemoveAll(p => p.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Remove Item of cart Successfully";

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Clear all Item of cart Successfully";
			return RedirectToAction("Index");
		}
	}
}
