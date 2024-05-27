using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.About
{
    public class CreateAboutRequest
    {
        public string? AboutName { get; set; }

        public string? Description { get; set; }

        public IFormFile? AboutImage { get; set; }

        public bool Status { get; set; }
    }
}
