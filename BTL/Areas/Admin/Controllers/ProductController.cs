using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
/*        public IActionResult Index()
        {
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

            return Json(new { data = products, TotalPages = totalPages, CurrentPage = page });
        }*/
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name",product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name",product.BrandId);
            if(ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ","-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p=>p.Slug == product.Slug);
                if(slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có!");
                    return View(product);
                }
                
                if(product.ImageUpload != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
                        string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);

                        FileStream fs =  new FileStream(filePath,FileMode.Create);
                        await product.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                        product.Image = imageName;
                    }
                
                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thành công";
                return RedirectToAction("Index");   
            }
            else
            {
                TempData["error"] = "Model đang lỗi";
                List<string> errors = new List<string>();
                foreach(var value in ModelState.Values){
                    foreach(var error in value.Errors) {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            
            return View(product);

        }
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product, int Id)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }

                _dataContext.Update(product);
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

            return View(product);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if (!string.Equals(product.Name,"noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                
                string oldfileImage = Path.Combine(uploadsDir, product.Image);
                if(System.IO.File.Exists(oldfileImage))
                {
                    System.IO.File.Delete(oldfileImage);
                }
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã xoá";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int productId, int newStatus)
        {
            var product = await _dataContext.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            product.Status = newStatus;
            _dataContext.Update(product);
            await _dataContext.SaveChangesAsync();

            return Ok(new { status = newStatus });
        }


    }
}
    

