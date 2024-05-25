using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.GemPriceList
{
    public class UpdateGemPriceListRequest
    {
        public int GemPriceListId { get; set; }

        public string Cut { get; set; } = null!;

        public string Clarity { get; set; } = null!;

        public string CaratWeight { get; set; } = null!;

        public string Color { get; set; } = null!;

        public decimal Price { get; set; }
        public Guid GemId { get; set; }
        public bool Active { get; set; }
        public DateTime effectDate { get; set; }


    }
}
