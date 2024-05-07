using BTL.Models;
using BTL.Repository;
using BTL.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BTL.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}else if (Cartitems.Sum(x=>x.Quantity)<=0)
			{
				TempData["error"] = "Tạo thất bại, giỏ hàng trống";
				return RedirectToAction("Index", "Cart");
			}
			else
			{
				var orderCode=Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = orderCode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				
				foreach(var cartItem in Cartitems)
				{
					var orderDetail = new OrderDetails();
					orderDetail.UserName = userEmail;
					orderDetail.OrderCode = orderCode;
					orderDetail.ProductId = cartItem.ProductId;
					orderDetail.Price = cartItem.Price;
					orderDetail.Quantity = cartItem.Quantity;
					_dataContext.Add(orderDetail);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Tạo thành công, chờ duyệt đơn hàng";
				return RedirectToAction("Index","Cart");

			}
			return View();
		}
	}
}
