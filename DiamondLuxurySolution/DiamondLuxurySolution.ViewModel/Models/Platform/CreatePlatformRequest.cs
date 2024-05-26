using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;


namespace DiamondLuxurySolution.ViewModel.Models.Platform
{
    public class CreatePlatformRequest
    {
        public string? PlatformName { get; set; }

        public string? PlatformUrl { get; set; }

        public IFormFile? PlatformLogo { get; set; }

        public bool Status { get; set; }
    }
}
