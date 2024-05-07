using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BTL.Controllers
{
    public class BrandController : Controller
    {
		private readonly DataContext _dataContext;

		public BrandController(DataContext context)
		{
			_dataContext = context;
		}

		public async Task<IActionResult> Index(string Slug = "")
        {
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			BrandModel brand = _dataContext.Brands.Where(c => (c.Slug == Slug && c.Status ==1)).FirstOrDefault();
			if(brand == null)
			{
				return RedirectToAction("Index");
			}
			var productsByBrand = _dataContext.Products.Where(p => p.BrandId == brand.Id);

			return View(await productsByBrand.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
