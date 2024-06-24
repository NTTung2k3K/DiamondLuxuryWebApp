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

        public string? AboutName { get; set; }
        public string? AboutEmail { get; set; }
        public string? AboutAddress { get; set; }
        public string? AboutPhoneNumber { get; set; }

        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
