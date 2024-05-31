using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Material
{
    public class MaterialVm
    {
        public Guid MaterialId { get; set; }

        public string? MaterialName { get; set; }

        public string? Description { get; set; }

        public string? Color { get; set; }
        public string? MaterialImage { get; set; }

        public bool Status { get; set; }
        public decimal? Price { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? EffectDate { get; set; }



    }
}
