using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Warranty
{
    public class CreateWarrantyRequest
    {
        public string WarrantyName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DateActive { get; set; }

        public DateTime DateExpired { get; set; }

        public bool Status { get; set; }
    }
}
