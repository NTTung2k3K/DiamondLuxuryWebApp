using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Warranty
{
    public class WarrantyVm
    {
        public string WarrantyId { get; set; }

        public string? WarrantyName { get; set; }

        public string? Description { get; set; }

        public DateTime DateActive { get; set; }

        public DateTime DateExpired { get; set; }

        public bool Status { get; set; }
    }
}
