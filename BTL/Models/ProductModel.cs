using BTL.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models
{
    public class ProductModel
    {
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Tên sản phẩm")]
		public string Name { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên Mô tả sản phẩm")]
		public string Description { get; set; }

		public string Slug { get; set; }

		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public int BrandId { get; set; }
		public int CategoryId { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }

		public string Image { get; set; }

		public int Status { get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile ImageUpload { get; set; }
	}
}
