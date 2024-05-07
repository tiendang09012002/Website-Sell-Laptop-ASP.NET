using System.ComponentModel.DataAnnotations;

namespace BTL.Models
{
	public class OrderModel
	{
        [Key]
        public string OrderCode { get; set; }
		public string UserName { get; set; }

		public DateTime CreatedDate { get; set; }

		public int Status { get; set; }
	}
}
