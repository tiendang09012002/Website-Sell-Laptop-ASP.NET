using System.ComponentModel.DataAnnotations;

namespace BTL.Models
{
    public class BrandModel
    {
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Tên thương hiệu")]
		public string Name { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Mô tả thương hiệu")]
		public string Description { get; set; }

		public string Slug { get; set; }

		public string at {  get; set; }
		public int Status { get; set; }
    }
}
