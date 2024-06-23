using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.BackgroundServiceHost.Service
{
	public interface IGemPriceListDefault
	{
		Task CreateDefaultGemPriceList(CancellationToken cancellationToken);
	}
}
