using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Slide
{
    public class CreateSlideRequest
    {
        public string SlideName { get; set; } = null!;

        public string? Description { get; set; }

        public string SlideUrl { get; set; } = null!;

        public IFormFile SlideImage { get; set; } = null!;

        public bool Status { get; set; }
    }
}
