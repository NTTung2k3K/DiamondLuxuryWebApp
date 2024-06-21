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
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fnhan-cuoi-slide.png?alt=media&token=23f6647f-0005-4eed-9a0c-09cb09fd7e95",
				};

				status = await _context.Slides.AddAsync(AddWeddingJewelryRing);

				var AddDiamond = new Data.Entities.Slide()
				{
					SlideName = Diamond,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fkim-cuong-slide.png?alt=media&token=1f85d0d1-4d4d-493b-88f2-b005ea80e406",
				};

				status = await _context.Slides.AddAsync(AddDiamond);

				var AddNecklace = new Data.Entities.Slide()
				{
					SlideName = Necklace,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fday-chuyen-slide.png?alt=media&token=b812bc2d-775f-4db1-ad73-acfceb838441",
				};

				status = await _context.Slides.AddAsync(AddNecklace);

				var AddEarring = new Data.Entities.Slide()
				{
					SlideName = Earring,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fbong-tai-slide.png?alt=media&token=ad37a5c3-7739-42b3-9e75-228d07e8cca8",
				};

				status = await _context.Slides.AddAsync(AddEarring);

				var AddRingMan = new Data.Entities.Slide()
				{
					SlideName = RingMan,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fnhan-nam-slide.png?alt=media&token=935b89b5-ecf0-4e8a-8266-4840144d7991",
				};

				status = await _context.Slides.AddAsync(AddRingMan);

				var AddRingWoman = new Data.Entities.Slide()
				{
					SlideName = RingWoman,
					Status = true,
					SlideImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fnhan-nu-slide.png?alt=media&token=5b3b3632-f73a-46dc-9906-1b7956d61300",
				};

				status = await _context.Slides.AddAsync(AddRingWoman);

				_context.SaveChanges();
			}

		}
	}
}
