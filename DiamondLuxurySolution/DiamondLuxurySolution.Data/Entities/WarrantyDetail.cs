using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class WarrantyDetail
    {
        public int WarrantyDetailId { get; set; }
        public string WarrantyDetailName { get; set; }
        public string WarrantyType { get; set; }

        public DateTime? ReceiveProductDate { get; set; }   
        public DateTime? ReturnProductDate { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string? Image { get; set; }

        public string WarrantyId { get; set; }
        public Warranty Warranty { get; set; }
    }
}
