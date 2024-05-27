using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.GemPriceList
{
    public class CreateGemPriceListRequest
    {
        public string? Cut { get; set; }

        public string? Clarity { get; set; }

        public string? CaratWeight { get; set; }

        public string? Color { get; set; }

        public string? Price { get; set; }

        public Guid GemId { get; set; }

        public bool Active { get; set; }
        public DateTime effectDate { get; set; }


    }
}
