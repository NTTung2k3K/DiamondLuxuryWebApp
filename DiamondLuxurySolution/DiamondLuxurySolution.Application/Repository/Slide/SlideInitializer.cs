using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Slide
{
	public class SlideInitializer : ISlideInitializer
	{
		private readonly LuxuryDiamondShopContext _context;
		public SlideInitializer(LuxuryDiamondShopContext context)
		{
			_context = context;
		}

		public async Task CreateDefaultSlide()
		{
			var MainSlide = DiamondLuxurySolution.Utilities.Constants.Systemconstant.SlideNameDefault.MainSlide.ToString();
			var Earring = DiamondLuxurySolution.Utilities.Constants.Systemconstant.SlideNameDefault.Earring.ToString();
			var RingMan = DiamondLuxurySolution.Utilities.Constants.Systemconstant.SlideNameDefault.RingWoman.ToString();
			var RingWoman = DiamondLuxurySolution.Utilities.Constants.Systemconstant.SlideNameDefault.RingMan.ToString();
			var Diamond = DiamondLuxurySolution.Utilities.Constants.Systemconstant.SlideNameDefault.Diamond.ToString();
			var Necklace = DiamondLuxurySolution.Utilities.Constants.Systemconstant.SlideNameDefault.Necklace.ToString();
			var WeddingJewelryRing = DiamondLuxurySolution.Utilities.Constants.Systemconstant.SlideNameDefault.WeddingJewelryRing.ToString();

			var slideCount = await _context.Slides.ToListAsync();
			if (slideCount.Count == 0)
			{
				var AddMainSlide1 = new Data.Entities.Slide()
				{
					SlideName = MainSlide,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide-default1.png?alt=media&token=55cdb01e-cbb2-43a9-9efd-9b7ddd9fbf4a",
				};

				var status = await _context.Slides.AddAsync(AddMainSlide1);

				var AddMainSlide2 = new Data.Entities.Slide()
				{
					SlideName = MainSlide,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide-default2.png?alt=media&token=faa7d372-6bf0-4af2-8f6a-9d2744467fa9",
				};

				status = await _context.Slides.AddAsync(AddMainSlide2);

				var AddMainSlide3 = new Data.Entities.Slide()
				{
					SlideName = MainSlide,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide-default3.png?alt=media&token=5ca8ac92-64fa-4ce7-b0a3-c914d3813c20",
				};

				status = await _context.Slides.AddAsync(AddMainSlide3);

				var AddWeddingJewelryRing = new Data.Entities.Slide()
				{
					SlideName = WeddingJewelryRing,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fsilde_nhan_cuoi.jpg?alt=media&token=1e37040b-1ddc-4d76-81d6-daedaae7d9e0",
				};

				status = await _context.Slides.AddAsync(AddWeddingJewelryRing);

				var AddDiamond = new Data.Entities.Slide()
				{
					SlideName = Diamond,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide_kim_cuong.jpg?alt=media&token=bc95407a-93bd-49fa-a7b5-50e14d348f3b",
				};

				status = await _context.Slides.AddAsync(AddDiamond);

				var AddNecklace = new Data.Entities.Slide()
				{
					SlideName = Necklace,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide_mat_day_chuyen.jpg?alt=media&token=3d157a12-3530-47fe-8de8-f8d118979a57",
				};

				status = await _context.Slides.AddAsync(AddNecklace);

				var AddEarring = new Data.Entities.Slide()
				{
					SlideName = Earring,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide_hoa_tai.jpg?alt=media&token=a8970dca-d5ec-4567-9d0f-a10e43e7a4ec",
				};

				status = await _context.Slides.AddAsync(AddEarring);

				var AddRingMan = new Data.Entities.Slide()
				{
					SlideName = RingMan,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide_nhan_nam.jpg?alt=media&token=0caffc41-7988-45bf-ba49-412ba0470508",
				};

				status = await _context.Slides.AddAsync(AddRingMan);

				var AddRingWoman = new Data.Entities.Slide()
				{
					SlideName = RingWoman,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fslide_nhan_nu.jpg?alt=media&token=24ffb6ae-27a8-4640-8157-cdb94144405a",
				};

				status = await _context.Slides.AddAsync(AddRingWoman);

				_context.SaveChanges();
			}

		}
	}
}
