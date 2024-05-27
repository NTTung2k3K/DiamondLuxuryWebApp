using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.MaterialPriceList
{
    public class CreateMaterialPriceListRequest
    {
        public string? BuyPrice { get; set; }

        public string? SellPrice { get; set; }
        public Guid MaterialId { get; set; }
        public bool Active { get; set; }
        public DateTime effectDate { get; set; }
    }
}
