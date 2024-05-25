using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Material
{
    public class CreateMaterialRequest
    {
        public string MaterialName { get; set; } = null!;

        public string? Description { get; set; }

        public string? Color { get; set; }
        public int Weight { get; set; }

        public IFormFile? MaterialImage { get; set; }

        public bool Status { get; set; }
    }
}
