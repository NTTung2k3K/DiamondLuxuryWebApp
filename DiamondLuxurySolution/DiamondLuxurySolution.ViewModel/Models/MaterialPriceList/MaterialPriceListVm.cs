using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondLuxurySolution.Data.Entities;

namespace DiamondLuxurySolution.ViewModel.Models.MaterialPriceList
{
    public class MaterialPriceListVm
    {
        public int MaterialPriceListId { get; set; }
        public decimal BuyPrice { get; set; }

        public decimal SellPrice { get; set; }
        public bool Active { get; set; }
        public DateTime effectDate { get; set; }
        public Data.Entities.Material? MaterialVm { get; set; }  
    }
}
