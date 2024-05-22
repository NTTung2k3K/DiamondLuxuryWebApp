﻿using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class GemPriceList
{
    public int GemPriceListId { get; set; }

    public string Cut { get; set; } = null!;

    public string Clarity { get; set; } = null!;

    public string CaratWeight { get; set; } = null!;

    public string Color { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<GemPriceListDetail> GemPriceListDetails { get; set; } = new List<GemPriceListDetail>();
}