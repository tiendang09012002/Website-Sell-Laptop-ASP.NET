using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.ToListAsync());
        }
		public async Task<IActionResult> ViewOrder(string ordercode)
		{
			var DetailOrder = await _dataContext.OrderDetails.Include(od=>od.Product).Where(od=>od.OrderCode== ordercode).ToListAsync();
			return View(DetailOrder);
		}
        public async Task<IActionResult> Delete(string orderCode)
        {
            // Tìm tất cả các chi tiết đơn hàng có cùng OrderCode
            var orderDetails = await _dataContext.OrderDetails
                .Where(od => od.OrderCode == orderCode)
                .ToListAsync();

            // Nếu không tìm thấy chi tiết đơn hàng, trả về NotFound hoặc thông báo lỗi
            if (orderDetails == null || !orderDetails.Any())
            {
                TempData["error"] = "Không tìm thấy chi tiết đơn hàng";
                return RedirectToAction("Index");
            }

            // Xóa tất cả các chi tiết đơn hàng
            _dataContext.OrderDetails.RemoveRange(orderDetails);

            // Tìm và xóa đơn hàng chính
            var order = await _dataContext.Orders.FindAsync(orderCode);
            if (order == null)
            {
                TempData["error"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index");
            }
            _dataContext.Orders.Remove(order);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Đơn hàng đã được xoá thành công";
            return RedirectToAction("Index");
        }

    }
}
