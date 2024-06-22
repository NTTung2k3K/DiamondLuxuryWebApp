using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Platform
{
    public class ProductSaleChart
    {
        public DateTime DateSale { get; set; }
        public List<ProductInfo> ListProduct { get; set; }

    }
    public class ProductInfo
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
