using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Warehouse
{
    public class UpdateWarehouseRequest
    {
        public int WareHouseId { get; set; }

        public string WareHouseName { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }
    }
}
