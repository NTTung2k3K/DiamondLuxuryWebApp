using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Category
{
	public class CategoryInitializer : ICategoryInitializer
	{

		private readonly LuxuryDiamondShopContext _context;
		public CategoryInitializer(LuxuryDiamondShopContext context)
		{
			_context = context;
		}
		public async Task CreateDefaultCategory()
		{
			var Diamond = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CategoryNameDefault.Diamond.ToString();
			var WeddingJewelry = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CategoryNameDefault.WeddingJewelry.ToString();
			var Ring = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CategoryNameDefault.Ring.ToString();
			var Necklace = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CategoryNameDefault.Necklace.ToString();
			var Earring = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CategoryNameDefault.Earring.ToString();
			var Bracelet = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CategoryNameDefault.Bracelet.ToString();
			var Bangles = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CategoryNameDefault.Bangles.ToString();

			var AddRingforMan = new Data.Entities.Category()
			{
				CategoryName = Ring,
				CategoryType = "Nam",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fnhan-nam.png?alt=media&token=155c1e1f-fec7-4e90-8476-8dd8ce024fd5",
			};

			var status = await _context.Categories.AddAsync(AddRingforMan);

			var AddRingforWoman = new Data.Entities.Category()
			{
				CategoryName = Ring,
				CategoryType = "Nữ",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fnhan-nu.png?alt=media&token=bf9c5d9f-0403-4560-a421-acfc40e6af43",
			};

			status = await _context.Categories.AddAsync(AddRingforWoman);

			var AddDiamond = new Data.Entities.Category()
			{
				CategoryName = Diamond,
				CategoryType = "",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fkim-cuong-tu-nhien.png?alt=media&token=8881476b-77c9-4aa6-bc81-44b2731f340c",
			};

			status = await _context.Categories.AddAsync(AddDiamond);

			var AddWeddingJewelryRing = new Data.Entities.Category()
			{
				CategoryName = WeddingJewelry,
				CategoryType = "Nhẫn Cưới",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fnhan-cuoi.png?alt=media&token=2440b9c6-405b-45dd-b5be-3282b702a03e",
			};

			status = await _context.Categories.AddAsync(AddWeddingJewelryRing);

			var AddWeddingJewelryProposal = new Data.Entities.Category()
			{
				CategoryName = WeddingJewelry,
				CategoryType = "Nhẫn Cầu Hôn",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fnhan-cau-hon.png?alt=media&token=3231a9b4-8726-4548-99a7-eeb149644ab2",
			};

			status = await _context.Categories.AddAsync(AddWeddingJewelryProposal);

			var AddNecklace = new Data.Entities.Category()
			{
				CategoryName = Necklace,
				CategoryType = "",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fday-chuyen.png?alt=media&token=3841f759-a261-4807-9297-c3706b3c68b5",
			};

			status = await _context.Categories.AddAsync(AddNecklace);

			var AddEarring = new Data.Entities.Category()
			{
				CategoryName = Earring,
				CategoryType = "",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fbong-tai.png?alt=media&token=e4de378d-70bb-417a-ad63-2aa21cd04f89",
			};

			status = await _context.Categories.AddAsync(AddEarring);

			var AddBracelet = new Data.Entities.Category()
			{
				CategoryName = Bracelet,
				CategoryType = "",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvong-tay.png?alt=media&token=38ff5bd3-94a0-4ff7-b391-66c1e9da3ba6",
			};

			status = await _context.Categories.AddAsync(AddBracelet);

			var AddBangles = new Data.Entities.Category()
			{
				CategoryName = Bangles,
				CategoryType = "",
				Status = true,
				CategoryImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Flac-tay.png?alt=media&token=3eed5e32-0524-4182-9b27-bdaf044c625d",
			};

			status = await _context.Categories.AddAsync(AddBangles);

			_context.SaveChanges();
		}
	}
}
