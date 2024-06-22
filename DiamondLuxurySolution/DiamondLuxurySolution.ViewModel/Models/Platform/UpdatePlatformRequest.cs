using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Platform
{
    public class UpdatePlatformRequest
    {
        public int PlatformId { get; set; }
        [Required(ErrorMessage = "Cần nhập tên nền tảng")]
        public string? PlatformName { get; set; }
            
        public string? PlatformUrl { get; set; }

        public IFormFile? PlatformLogo { get; set; }

        public bool Status { get; set; }
    }
}
