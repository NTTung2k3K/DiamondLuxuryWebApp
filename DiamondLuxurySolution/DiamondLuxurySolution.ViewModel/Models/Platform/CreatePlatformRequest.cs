using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;


namespace DiamondLuxurySolution.ViewModel.Models.Platform
{
    public class CreatePlatformRequest
    {
        [Required(ErrorMessage = "Cần nhập tên nền tảng")]
        public string? PlatformName { get; set; }

        public string? PlatformUrl { get; set; }

        [Required(ErrorMessage = "Cần nhập hình ảnh nền tảng")]
        public IFormFile PlatformLogo { get; set; }

        public bool Status { get; set; }
    }
}
