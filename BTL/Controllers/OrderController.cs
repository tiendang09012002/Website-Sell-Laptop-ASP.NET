using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index()
        {
            List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
            // Lấy danh sách tất cả các tên người dùng duy nhất
            var userNames = await _dataContext.OrderDetails.Select(od => od.UserName).Distinct().ToListAsync();

            // Tạo một danh sách để lưu trữ tất cả các chi tiết đơn hàng
            var allOrders = new List<OrderDetails>();

            // Duyệt qua từng tên người dùng
            foreach (var userName in userNames)
            {
                // Lấy tất cả các chi tiết đơn hàng có tên người dùng trùng khớp
                var ordersForUser = await _dataContext.OrderDetails
                        .Include(od => od.Product)
                        .Where(od => od.UserName == userName)
                        .ToListAsync();

                // Thêm các chi tiết đơn hàng vào danh sách chung
                allOrders.AddRange(ordersForUser);
            }

            return View(allOrders);
        }
    }
}
