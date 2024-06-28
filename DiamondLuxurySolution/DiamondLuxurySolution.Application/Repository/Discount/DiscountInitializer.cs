using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Discount
{
	public class DiscountInitializer : IDiscountInitializer
	{
		private readonly LuxuryDiamondShopContext _context;
		public DiscountInitializer(LuxuryDiamondShopContext context)
		{
			_context = context;
		}
		public async Task CreateDefaultDiscount()
		{
			var discountCount = await _context.Discounts.ToListAsync();
			if (discountCount.Count == 0)
			{
				var discounts = new List<Data.Entities.Discount>();

				for (int i = 1; i <= 10; i++)
				{
					string discountId = $"DC{i:D4}"; 

					discounts.Add(new Data.Entities.Discount
					{
						DiscountId = discountId,
						DiscountName = $"Chiết khấu {i}",
						PercentSale = i * 0.5,
						Status = true,
						From = (i - 1) * 1000 + 1,
						To = i * 1000
					});
				}

				await _context.Discounts.AddRangeAsync(discounts);

				var AddDiscountOut = new Data.Entities.Discount()
				{
					DiscountId = "DC0011",
					DiscountName = "Chiết khấu 11",
					PercentSale = 5,
					Status = true,
					From = 10001,
					To = int.MaxValue,
				};

				await _context.Discounts.AddAsync(AddDiscountOut);

				await _context.SaveChangesAsync();
			}
		}


	}
}
