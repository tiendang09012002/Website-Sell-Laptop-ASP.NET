using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTL.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BrandModel brand)
		{

			if (ModelState.IsValid)
			{
                brand.Slug = brand.Name.Replace(" ", "-");
				var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Thương hiệu đã có!");
					return View(brand);
				}

				_dataContext.Add(brand);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm thành công";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model đang lỗi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
			}
		}

        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
           
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand, int Id)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Sửa thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model đang lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            
            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "THương hiệu đã xoá";
            return RedirectToAction("Index");
        }
    }
}
