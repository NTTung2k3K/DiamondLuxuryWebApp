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
        public string PlatformName { get; set; } = null!;

        public string PlatformUrl { get; set; } = null!;

        public IFormFile PlatformLogo { get; set; } = null!;
    }
}
