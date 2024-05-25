using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Platform
{
    public class PlatfromVm
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; } = null!;

        public string? PlatformUrl { get; set; }

        public string? PlatformLogo { get; set; }

        public bool Status { get; set; }
    }
}
