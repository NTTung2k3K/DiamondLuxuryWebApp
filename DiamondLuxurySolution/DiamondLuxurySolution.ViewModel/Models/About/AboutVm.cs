using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.About
{
    public class AboutVm
    {
        public int AboutId { get; set; }

        public string AboutName { get; set; } = null!;

        public string? Description { get; set; }

        public string AboutImage { get; set; } = null!;
    }
}
