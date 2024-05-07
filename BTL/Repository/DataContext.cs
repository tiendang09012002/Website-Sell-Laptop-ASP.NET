using BTL.Models;
using BTL.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BTL.Repository
{
	public class DataContext : IdentityDbContext<ApplicationUser>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}

		public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }

	}

	public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
	{
		public DataContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			optionsBuilder.UseSqlServer("Data Source=ADMIN\\MAY1;Initial Catalog=Shopping2;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

			return new DataContext(optionsBuilder.Options);
		}
	}
}
