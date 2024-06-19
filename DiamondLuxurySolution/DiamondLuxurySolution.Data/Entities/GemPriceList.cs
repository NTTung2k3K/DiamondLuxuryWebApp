using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class GemPriceList
{
    public int GemPriceListId { get; set; }

    public string? Cut { get; set; }

    public string? Clarity { get; set; }

    public string? CaratWeight { get; set; }

    public string? Color { get; set; }

    public decimal? Price { get; set; }
	public List<Gem> Gems { get; set; }
	public bool Active { get; set; }
    public DateTime effectDate {  get; set; } 
}
