using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Slide
{
    public class SlideViewModel

    {
        public int SlideId { get; set; }

        public string? SlideName { get; set; }

        public string? Description { get; set; }

        public string? SlideUrl { get; set; }

        public string? SlideImage { get; set; }

        public bool Status { get; set; }
    }
}
