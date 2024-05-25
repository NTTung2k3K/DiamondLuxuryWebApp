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

        public string MaterialName { get; set; } = null!;

        public string? Description { get; set; }

        public string? Color { get; set; }
        public int Weight { get; set; }

        public string? MaterialImage { get; set; }

        public bool Status { get; set; }
    }
}
