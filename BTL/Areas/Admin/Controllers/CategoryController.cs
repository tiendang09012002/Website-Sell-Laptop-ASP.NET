using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTL.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{

			if (ModelState.IsValid)
			{
                category.Slug = category.Name.Replace(" ", "-");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Danh mục đã có!");
					return View(category);
				}

				

				_dataContext.Add(category);
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
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
           
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category, int Id)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                _dataContext.Update(category);
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
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Danh mục đã xoá";
            return RedirectToAction("Index");
        }
    }
}
