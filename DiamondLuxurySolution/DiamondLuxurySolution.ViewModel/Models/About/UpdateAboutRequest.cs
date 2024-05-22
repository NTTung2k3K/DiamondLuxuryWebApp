using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.About
{
    public class UpdateAboutRequest
    {
        public int AboutId { get; set; }

        public string AboutName { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile AboutImage { get; set; } = null!;
    }
}
