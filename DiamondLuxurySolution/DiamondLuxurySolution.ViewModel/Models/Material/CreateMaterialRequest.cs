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
        public string? MaterialName { get; set; }

        public string? Description { get; set; }

        public string? Color { get; set; }

        public IFormFile? MaterialImage { get; set; }

        public bool Status { get; set; }

        public DateTime EffectDate { get; set; }

        public string? Price { get; set; }

        public string? Weight { get; set; }

    }
}
