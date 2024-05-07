using BTL.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if(!_context.Products.Any())
			{
				CategoryModel laptop = new CategoryModel { Name = "Laptop", Slug = "laptop", Description = "Laptop, Máy tính sách tay", Status = 1 };
				CategoryModel pc = new CategoryModel { Name = "PC", Slug = "pc", Description = "Máy tính để bàn", Status = 1 };
				CategoryModel ipad = new CategoryModel { Name = "Ipad", Slug = "ipad", Description = "Máy tính bảng", Status = 1 };
				CategoryModel services = new CategoryModel { Name = "Services", Slug = "sercives", Description = "Linh kiện", Status = 1 };

				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Hãng Apple", Status = 1 };
				BrandModel asus = new BrandModel { Name = "Asus", Slug = "asus", Description = "Hãng Asus", Status = 1 };
				BrandModel acer = new BrandModel { Name = "Acer", Slug = "acer", Description = "Hãng Acer", Status = 1 };
				BrandModel lenovo = new BrandModel { Name = "Lenovo", Slug = "lenovo", Description = "Hãng Lenovo", Status = 1 };
				BrandModel dell = new BrandModel { Name = "Dell", Slug = "dell", Description = "Hãng Dell", Status = 1 };
				BrandModel hp = new BrandModel { Name = "HP", Slug = "msi", Description = "Hãng HP", Status = 1 };

				_context.Products.AddRange(

					new ProductModel
					{
						Name = "LAPTOP APPLE MACBOOK PRO 13 (Z16R0003V)",
						Slug = "LAPTOP APPLE MACBOOK PRO 13 (Z16R0003V)",
						Description = "LAPTOP APPLE MACBOOK PRO 13 (Z16R0003V) (APPLE M2 /8C CPU/10C GPU/16GB/256GB SSD/13.3/MAC OS/XÁM)" +
					",CPU: Apple M2,RAM: 16GB,Ổ cứng: 256GB SSD,VGA: Onboard,Màn hình: 13.3 inch Retina IPS,HĐH: Mac OS",
						Image = "1.png",
						Category = laptop,
						Brand = apple,
						Price = 36999000,
						Quantity = 10,
						Status = 1,
					},

					new ProductModel
					{
						Name = "LAPTOP ASUS X1504VA-NJ069W",
						Slug = "LAPTOP ASUS X1504VA-NJ069W",
						Description = "LAPTOP ASUS X1504VA-NJ069W (I3 1315U/8GB RAM/512GB SSD/15.6 FHD/WIN11/BẠC)" +
					",CPU: Intel Core i3-1315U (Up to 4.50GHz, 10MB Cache),RAM: 8GB,Ổ cứng: 512GB M.2 NVMe™ PCIe®,VGA: Intel Iris Xe Graphics,Màn hình: 15.6 inch FHD (1920 x 1080) 16:9; 250nits; 45% NTSC,Tính năng: Bảo mật vân tay",
						Image = "2.jpg",
						Category = laptop,
						Brand = asus,
						Price = 10499000,
                        Quantity = 10,
                        Status = 1,
                    },

					new ProductModel
					{
						Name = "LAPTOP LENOVO YOGA SLIM 7 14IMH9 (83CV001UVN)",
						Slug = "LAPTOP LENOVO YOGA SLIM 7 14IMH9 (83CV001UVN)",
						Description = "LAPTOP LENOVO YOGA SLIM 7 14IMH9 (83CV001UVN) (ULTRA7 155H/32GB RAM/512GB SSD/14 WUXGA/WIN11/OFFICE H&S/XÁM)" +
					",CPU: Intel® Core™ i5-1335U,RAM: 16GB LPDDR5 6400MHz (Hàn liền không nâng cấp được),Ổ cứng: 512GB SSD PCIe NVMe ,VGA: Intel® UHD Graphics,Màn hình: 14 WUXGA (1920 x 1200) IPS 60Hz Acer ComfyView™,Màu: Bạc",
						Image = "3.jpg",
						Category = laptop,
						Brand = acer,
						Price = 15999000,
                        Quantity = 10,
                        Status = 1,
                    },

					new ProductModel
					{
						Name = "LAPTOP ACER ASPIRE 5 A514-56P-55K5 (NX.KHRSV.003)",
						Slug = "LAPTOP ACER ASPIRE 5 A514-56P-55K5 (NX.KHRSV.003)",
						Description = "LAPTOP ACER ASPIRE 5 A514-56P-55K5 (NX.KHRSV.003) (I5 1335U/16GB RAM/512GB SSD/14.0 INCH WUXGA IPS/WIN11/XÁM)" +
					",CPU: Intel® Core™ Ultra 7 155H; 16C (6P + 8E + 2LPE),RAM: 32GB (2x16GB) SO-DIMM DDR5-7467MHz (Hàn liền không nâng cấp được),Ổ cứng: 512TB SSD M.2 PCIe 4.0x4 NVMe (Nâng cấp thay thế),VGA: Intel® Arc™ Graphics,Màn hình: 14 WUXGA (1920x1200) OLED 400nits Glossy; 100% DCI-P3;60Hz;Eyesafe®;Dolby® Vision™;DisplayHDR™ True Black 500,Màu: Xám",
						Image = "4.jpg",
						Category = laptop,
						Brand = lenovo,
						Price = 27999000,
                        Quantity = 10,
                        Status = 1,
                    },

					new ProductModel
					{
						Name = "LAPTOP DELL WORKSTATION MOBILE PRECISION 3581 VPRO ENTERPRISE (71023331)",
						Slug = "LAPTOP DELL WORKSTATION MOBILE PRECISION 3581 VPRO ENTERPRISE (71023331)",
						Description = "LAPTOP DELL WORKSTATION MOBILE PRECISION 3581 VPRO ENTERPRISE (71023331) (I7-13800H/16GB RAM/512GB SSD/RTX A500 4GB/15.6 INCH FHD/UBUNTU/XÁM)" +
					",CPU: Intel Core i7-13800H,RAM: 16GB (16GBx1) SO-DIMM DDR5-4800MHz (Tối đa 64GB),Ổ cứng: 512GB SSD M.2 PCIe 4.0x4 NVMe (Còn trống 1 khe),VGA: NVIDIA® GeForce RTX™ A500 4GB GDDR6,Màn hình: 15.6; FHD (1920x1080) IPS 250 nits Anti-glare,Màu: Xám",
						Image = "5.jpg",
						Category = laptop,
						Brand = dell,
						Price = 44349000,
                        Quantity = 10,
                        Status = 1,
                    },

					new ProductModel
					{
						Name = "LAPTOP HP PROBOOK 450 G10 (9H1N8PT)",
						Slug = "LAPTOP HP PROBOOK 450 G10 (9H1N8PT)",
						Description = "LAPTOP HP PROBOOK 450 G10 (9H1N8PT) (I5 1335U/16GB RAM/512GB SSD/15.6 FHD/RTX2050 4GB/WIN11/BẠC)" +
					",CPU: Intel® Core™ i5-1335U,RAM: 16 GB DDR4-3200 MHz RAM (1 x 16 GB)(Còn trống 1 khe),Ổ cứng: 512 GB PCIe® NVMe™ SSD (Nâng cấp thay thế),VGA: NVIDIA® GeForce RTX™ 2050 Laptop GPU,Màn hình: 15.6 FHD (1920 x 1080); IPS; narrow bezel; anti-glare; 250 nits; 45% NTSC,Màu: Xám",
						Image = "6.jpg",
						Category = laptop,
						Brand = hp,
						Price = 23999000,
                        Quantity = 10,
                        Status = 1,
                    },

					new ProductModel
					{
						Name = "PC ASUS ROG STRIX G35DX (G35DX-VN003W)",
						Slug = "PC ASUS ROG STRIX G35DX (G35DX-VN003W)",
						Description = "PC ASUS ROG STRIX G35DX (R7-5800X/16GB RAM/1TB SSD/RTX3070 8GB/WL+BT/WIN 11) (G35DX-VN003W)" +
					",CPU: AMD Ryzen 7 5800X,RAM: 16GB,Ổ cứng: 1 TB SSD,VGA: RTX 3070,Wifi: Có,Bluetooth: Có",
						Image = "7.jpg",
						Category = pc,
						Brand = asus,
						Price = 26999000,
                        Quantity = 10,
                        Status = 1,
                    },

					new ProductModel
					{
						Name = "MÁY TÍNH BẢNG APPLE IPAD PRO 11 M2 (MNXD3ZA/A)",
						Slug = "MÁY TÍNH BẢNG APPLE IPAD PRO 11 M2 (MNXD3ZA/A)",
						Description = "MÁY TÍNH BẢNG APPLE IPAD PRO 11 M2 (MNXD3ZA/A) (128GB/11.0 INCH/WIFI/XÁM/2022)" +
					",CPU: Apple M2,RAM: 8GB,Bộ nhớ : 128GB,Kết nối : Wifi,Kích thước : 11,Màu : Xám",
						Image = "8.jpg",
						Category = ipad,
						Brand = apple,
						Price = 19999000,
                        Quantity = 10,
                        Status = 1,
                    },
					new ProductModel
					{
						Name = "NGUỒN ASUS TUF GAMING 1200W GOLD ATX3.0",
						Slug = "NGUỒN ASUS TUF GAMING 1200W GOLD ATX3.0",
						Description = "NGUỒN ASUS TUF GAMING 1200W GOLD ATX3.0(PCI GEN 5.0 / 80 PLUS GOLD / MÀU ĐEN / FULL MODULAR)" +
					",Chứng nhận độ bền chuẩn quân sự,Ổ trục bi kép có độ bền gấp đôi so với thiết kế ổ trục bi thông thường,Một lớp phủ PCB bảo vệ bảng mạch khỏi độ ẩm cao; bụi bẩn",
						Image = "9.jpg",
						Category = services,
						Brand = asus,
						Price = 5999000,
                        Quantity = 10,
                        Status = 1,
                    },

					new ProductModel
					{
						Name = "CARD MÀN HÌNH ASUS ROG STRIX RTX 4080 SUPER-O16G-GAMING",
						Slug = "CARD MÀN HÌNH ASUS ROG STRIX RTX 4080 SUPER-O16G-GAMING",
						Description = "NGUỒN ASUS TUF GAMING 1200W GOLD ATX3.0(PCI GEN 5.0 / 80 PLUS GOLD / MÀU ĐEN / FULL MODULAR)" +
					",Nhân đồ họa: NVIDIA® GeForce RTX™ 4080 Super,Nhân CUDA: 10420,Dung lượng bộ nhớ: 16GB GDDR6X,Tốc độ bộ nhớ: 23 Gbps,Giao diện bộ nhớ: 256-bit,Nguồn khuyến nghị: 850W",
						Image = "10.jpg",
						Category = services,
						Brand = asus,
						Price = 39299000,
                        Quantity = 10,
                        Status = 1,
                    }
				);

				_context.SaveChanges();
			}
		}
	}
}
