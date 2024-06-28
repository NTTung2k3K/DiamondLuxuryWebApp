using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Discount
{
	public interface IDiscountInitializer
	{
		Task CreateDefaultDiscount();
	}
}
