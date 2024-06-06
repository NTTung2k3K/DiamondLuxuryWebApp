using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Gem;

namespace DiamondLuxurySolution.ViewModel.Models.GemPriceList
{
    public class GemPriceListVm
    {
        public int GemPriceListId { get; set; }

        public string? Cut { get; set; } 

        public string? Clarity { get; set; }

        public string? CaratWeight { get; set; }

        public string? Color { get; set; }

        public decimal Price { get; set; }
        public GemVm GemVm { get; set; } 
        public bool Active { get; set; }
        public DateTime effectDate { get; set; }


    }
}
