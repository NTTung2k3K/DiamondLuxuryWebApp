﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.BackgroundServiceHost.Service.GemPriceListService
{
    public interface IGemPriceListDefault
    {
        Task CreateDefaultGemPriceList(CancellationToken cancellationToken);

        Task CreateDefaultGemPriceListNatural(CancellationToken cancellationToken);
    }
}
