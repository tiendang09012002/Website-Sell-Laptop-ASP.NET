using BTL.Models;
using BTL.Models.ViewModels;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing.Printing;

namespace BTL.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;

		public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
			_dataContext = context;
		}

        public IActionResult Index()
        {
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			return View();
		}
        [HttpGet]
        public IActionResult GetList(int page = 1, int pageSize = 6)
        {
            // Tổng số sản phẩm
            var totalProducts = _dataContext.Products.Count();

            // Tính toán số trang dựa trên tổng số sản phẩm và kích thước trang
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Lấy danh sách sản phẩm cho trang hiện tại
            var products = _dataContext.Products
                        .Include("Category")
                        .Include("Brand")
                        .Where(p => p.Status == 1)
                        .OrderBy(p => p.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            return Json(new { data = products , TotalPages = totalPages, CurrentPage = page });
		}

        [HttpGet]
		public IActionResult Search(string searchText)
		{
			// Tìm kiếm trong danh sách sản phẩm theo tên, giá, brand hoặc category
			var searchResults = _dataContext.Products
				.Include(p => p.Category)
				.Include(p => p.Brand)
				.Where(p =>
					p.Name.Contains(searchText) ||
					p.Price.ToString().Contains(searchText) ||
					p.Brand.Name.Contains(searchText) ||
					p.Category.Name.Contains(searchText))
				.ToList();

			return Json(searchResults);
		}
		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
