using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Frame
{
    public class CreateFrameRequest
    {
        public string? NameFrame { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        public Guid MaterialId { get; set; }
    }
}
