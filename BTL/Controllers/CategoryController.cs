using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BTL.Controllers
{
    public class CategoryController : Controller
    {
		private readonly DataContext _dataContext;

		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}

		public async Task<IActionResult> Index(string Slug = "")
        {
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
			if(category == null)
			{
				return RedirectToAction("Index");
			}
			var productsByCategory = _dataContext.Products.Where(p => p.CategoryId == category.Id);

			return View(await productsByCategory.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
