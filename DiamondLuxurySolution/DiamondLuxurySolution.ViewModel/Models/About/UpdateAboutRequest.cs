using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.About
{
    public class UpdateAboutRequest
    {
        public int AboutId { get; set; }
        [Required(ErrorMessage = "Cần Thêm Thông Tin Liên Hệ")]
        public string? AboutName { get; set; }

        public string? Description { get; set; }

        public IFormFile? AboutImage { get; set; }

        public bool Status { get; set; }
    }
}
